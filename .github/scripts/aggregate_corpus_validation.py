#!/usr/bin/env python3
"""Aggregate full-corpus shard reports and enforce the pass-rate gate."""

from __future__ import annotations

import argparse
import html
import json
import urllib.request
import xml.etree.ElementTree as ET
from datetime import datetime, timezone
from pathlib import Path
from typing import Any


SCHEMA_VERSION = 1
NUGET_INDEX_URL = (
    "https://api.nuget.org/v3-flatcontainer/aspose.cad/index.json"
)


def utc_now() -> str:
    return datetime.now(timezone.utc).isoformat()


def load_reports(paths: list[Path]) -> list[dict[str, Any]]:
    reports = []
    for path in paths:
        payload = json.loads(path.read_text(encoding="utf-8"))
        if not isinstance(payload, dict):
            raise ValueError(f"Shard report must be an object: {path}")
        reports.append(payload)
    return sorted(reports, key=lambda item: int(item["shard_index"]))


def latest_stable_package_version(timeout: float = 10.0) -> str | None:
    try:
        with urllib.request.urlopen(NUGET_INDEX_URL, timeout=timeout) as response:
            payload = json.loads(response.read().decode("utf-8"))
    except Exception:
        return None
    versions = [
        str(version)
        for version in payload.get("versions", [])
        if "-" not in str(version)
    ]
    return versions[-1] if versions else None


def aggregate_reports(
    reports: list[dict[str, Any]],
    *,
    expected_shards: int,
    threshold: float,
    latest_package_version: str | None,
) -> dict[str, Any]:
    if expected_shards < 1:
        raise ValueError("expected_shards must be at least 1")
    if not 0.0 <= threshold <= 1.0:
        raise ValueError("threshold must be between 0 and 1")
    if len(reports) != expected_shards:
        raise ValueError(
            f"Expected {expected_shards} shard reports, found {len(reports)}"
        )
    indexes = [int(report["shard_index"]) for report in reports]
    if indexes != list(range(expected_shards)):
        raise ValueError(f"Unexpected shard indexes: {indexes}")

    schema_versions = {
        int(report.get("schema_version") or 0) for report in reports
    }
    validation_ids = {
        str(report.get("validation_id") or "") for report in reports
    }
    repositories = {
        str(report.get("repository") or "") for report in reports
    }
    discovered_totals = {
        int(report["discovered_total"]) for report in reports
    }
    package_versions = {str(report["package_version"]) for report in reports}
    commit_shas = {str(report.get("commit_sha") or "") for report in reports}
    shard_counts = {int(report["shard_count"]) for report in reports}
    if schema_versions != {SCHEMA_VERSION}:
        raise ValueError(f"Unsupported shard schema versions: {schema_versions}")
    if len(validation_ids) != 1 or not next(iter(validation_ids)):
        raise ValueError("Shard reports disagree on validation_id")
    if len(repositories) != 1 or not next(iter(repositories)):
        raise ValueError("Shard reports disagree on repository")
    if len(discovered_totals) != 1:
        raise ValueError("Shard reports disagree on discovered_total")
    if len(package_versions) != 1:
        raise ValueError("Shard reports disagree on package_version")
    if len(commit_shas) != 1 or not next(iter(commit_shas)):
        raise ValueError("Shard reports disagree on commit_sha")
    if shard_counts != {expected_shards}:
        raise ValueError(
            f"Shard reports disagree on shard_count: {shard_counts}"
        )

    for report in reports:
        selected_total = int(report["selected_total"])
        result_total = len(report.get("results", []))
        if result_total != selected_total:
            raise ValueError(
                f"Shard {report['shard_index']} contains {result_total} "
                f"results but reports selected_total={selected_total}"
            )

    selected_total = sum(int(report["selected_total"]) for report in reports)
    discovered_total = discovered_totals.pop()
    if selected_total != discovered_total:
        raise ValueError(
            f"Shard coverage mismatch: selected {selected_total}, "
            f"discovered {discovered_total}"
        )

    results = [
        result
        for report in reports
        for result in report.get("results", [])
    ]
    paths = [str(result["path"]) for result in results]
    if len(paths) != len(set(paths)):
        raise ValueError("Duplicate example paths found across shards")

    passed = sum(result.get("status") == "passed" for result in results)
    failed = len(results) - passed
    compile_passed = sum(
        result.get("compile_status") == "passed" for result in results
    )
    compile_failed = sum(
        result.get("compile_status") in {"failed", "timeout"}
        for result in results
    )
    runtime_passed = sum(
        result.get("runtime_status") == "passed" for result in results
    )
    runtime_failed = sum(
        result.get("runtime_status") in {"failed", "timeout"}
        for result in results
    )
    pass_rate = passed / len(results) if results else 0.0
    package_version = package_versions.pop()
    failures_by_stage: dict[str, int] = {}
    failures_by_category: dict[str, int] = {}
    for result in results:
        if result.get("status") == "passed":
            continue
        stage = str(result.get("stage") or "unknown")
        category = str(result.get("category") or "unknown")
        failures_by_stage[stage] = failures_by_stage.get(stage, 0) + 1
        failures_by_category[category] = (
            failures_by_category.get(category, 0) + 1
        )

    return {
        "schema_version": SCHEMA_VERSION,
        "validation_id": reports[0].get("validation_id"),
        "repository": reports[0].get("repository"),
        "commit_sha": commit_shas.pop(),
        "package_version": package_version,
        "latest_package_version": latest_package_version,
        "package_stale": (
            latest_package_version is not None
            and latest_package_version != package_version
        ),
        "generated_at": utc_now(),
        "shard_count": expected_shards,
        "total": len(results),
        "compile_passed": compile_passed,
        "compile_failed": compile_failed,
        "runtime_passed": runtime_passed,
        "runtime_failed": runtime_failed,
        "passed": passed,
        "failed": failed,
        "pass_rate": round(pass_rate, 6),
        "threshold": threshold,
        "gate_passed": bool(results) and pass_rate >= threshold,
        "failures_by_stage": dict(sorted(failures_by_stage.items())),
        "failures_by_category": dict(sorted(failures_by_category.items())),
        "failed_examples": [
            result for result in results if result.get("status") != "passed"
        ],
        "shards": [
            {
                "shard_index": report["shard_index"],
                "selected_total": report["selected_total"],
                "passed": report["passed"],
                "failed": report["failed"],
                "pass_rate": report["pass_rate"],
            }
            for report in reports
        ],
    }


def render_markdown(summary: dict[str, Any]) -> str:
    package_status = (
        f"stale; latest is `{summary['latest_package_version']}`"
        if summary["package_stale"]
        else "current"
        if summary["latest_package_version"]
        else "latest version unavailable"
    )
    lines = [
        "# Aspose.CAD Full-Corpus Validation",
        "",
        f"- Validation ID: `{summary['validation_id']}`",
        f"- Commit: `{summary['commit_sha']}`",
        f"- Package: `{summary['package_version']}` ({package_status})",
        f"- Examples: {summary['total']}",
        f"- Compile passed: {summary['compile_passed']}",
        f"- Compile failed: {summary['compile_failed']}",
        f"- Runtime passed: {summary['runtime_passed']}",
        f"- Runtime failed: {summary['runtime_failed']}",
        f"- Passed: {summary['passed']}",
        f"- Failed: {summary['failed']}",
        f"- Pass rate: {summary['pass_rate']:.2%}",
        f"- Required threshold: {summary['threshold']:.2%}",
        f"- Gate: {'PASS' if summary['gate_passed'] else 'FAIL'}",
        "",
        "## Failures By Stage",
        "",
        "| Stage | Count |",
        "|---|---:|",
    ]
    for stage, count in summary["failures_by_stage"].items():
        lines.append(f"| {stage} | {count} |")
    if not summary["failures_by_stage"]:
        lines.append("| None | 0 |")

    lines.extend(
        [
            "",
            "## Failed Examples",
            "",
            "| Path | Stage | Error |",
            "|---|---|---|",
        ]
    )
    for result in summary["failed_examples"][:200]:
        error = str(result.get("error") or "").replace("|", "\\|")
        lines.append(
            f"| `{result['path']}` | {result.get('stage')} | {error} |"
        )
    if not summary["failed_examples"]:
        lines.append("| None | - | - |")
    return "\n".join(lines) + "\n"


def render_html(summary: dict[str, Any]) -> str:
    rows = []
    for result in summary["failed_examples"][:200]:
        rows.append(
            "<tr>"
            f"<td>{html.escape(str(result['path']))}</td>"
            f"<td>{html.escape(str(result.get('stage') or ''))}</td>"
            f"<td>{html.escape(str(result.get('error') or ''))}</td>"
            "</tr>"
        )
    failure_rows = "\n".join(rows) or (
        '<tr><td colspan="3">No failures</td></tr>'
    )
    gate_class = "pass" if summary["gate_passed"] else "fail"
    return f"""<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Aspose.CAD Full-Corpus Validation</title>
  <style>
    body {{ font-family: sans-serif; margin: 2rem; }}
    .pass {{ color: #087830; }} .fail {{ color: #b42318; }}
    table {{ border-collapse: collapse; width: 100%; }}
    th, td {{ border: 1px solid #ccc; padding: .4rem; text-align: left; }}
  </style>
</head>
<body>
  <h1>Aspose.CAD Full-Corpus Validation</h1>
  <p class="{gate_class}"><strong>Gate:
    {'PASS' if summary['gate_passed'] else 'FAIL'}</strong></p>
  <ul>
    <li>Commit: <code>{html.escape(summary['commit_sha'])}</code></li>
    <li>Package: <code>{html.escape(summary['package_version'])}</code></li>
    <li>Total: {summary['total']}</li>
    <li>Compile passed: {summary['compile_passed']}</li>
    <li>Compile failed: {summary['compile_failed']}</li>
    <li>Runtime passed: {summary['runtime_passed']}</li>
    <li>Runtime failed: {summary['runtime_failed']}</li>
    <li>Passed: {summary['passed']}</li>
    <li>Failed: {summary['failed']}</li>
    <li>Pass rate: {summary['pass_rate']:.2%}</li>
    <li>Threshold: {summary['threshold']:.2%}</li>
  </ul>
  <h2>Failed Examples</h2>
  <table>
    <thead><tr><th>Path</th><th>Stage</th><th>Error</th></tr></thead>
    <tbody>{failure_rows}</tbody>
  </table>
</body>
</html>
"""


def write_junit(path: Path, summary: dict[str, Any]) -> None:
    suite = ET.Element(
        "testsuite",
        {
            "name": "full-corpus-validation-gate",
            "tests": "1",
            "failures": "0" if summary["gate_passed"] else "1",
            "errors": "0",
        },
    )
    case = ET.SubElement(
        suite,
        "testcase",
        {"classname": "corpus", "name": "pass-rate-threshold"},
    )
    if not summary["gate_passed"]:
        failure = ET.SubElement(
            case,
            "failure",
            {
                "type": "PassRateThreshold",
                "message": (
                    f"Pass rate {summary['pass_rate']:.2%} is below "
                    f"{summary['threshold']:.2%}"
                ),
            },
        )
        failure.text = json.dumps(summary["failures_by_stage"], sort_keys=True)
    path.parent.mkdir(parents=True, exist_ok=True)
    ET.ElementTree(suite).write(path, encoding="utf-8", xml_declaration=True)


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser()
    parser.add_argument("reports", nargs="+", type=Path)
    parser.add_argument("--expected-shards", type=int, required=True)
    parser.add_argument("--threshold", type=float, default=0.95)
    parser.add_argument("--out", required=True, type=Path)
    parser.add_argument("--markdown", required=True, type=Path)
    parser.add_argument("--html", required=True, type=Path)
    parser.add_argument("--junit", required=True, type=Path)
    parser.add_argument("--latest-package-version")
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    reports = load_reports(args.reports)
    latest = (
        args.latest_package_version
        if args.latest_package_version is not None
        else latest_stable_package_version()
    )
    summary = aggregate_reports(
        reports,
        expected_shards=args.expected_shards,
        threshold=args.threshold,
        latest_package_version=latest,
    )
    args.out.parent.mkdir(parents=True, exist_ok=True)
    args.out.write_text(
        json.dumps(summary, indent=2, sort_keys=True) + "\n",
        encoding="utf-8",
    )
    args.markdown.write_text(render_markdown(summary), encoding="utf-8")
    args.html.write_text(render_html(summary), encoding="utf-8")
    write_junit(args.junit, summary)
    print(
        f"Full corpus: passed={summary['passed']} failed={summary['failed']} "
        f"pass_rate={summary['pass_rate']:.2%} "
        f"threshold={summary['threshold']:.2%}"
    )
    return 0 if summary["gate_passed"] else 1


if __name__ == "__main__":
    raise SystemExit(main())
