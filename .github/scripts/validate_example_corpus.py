#!/usr/bin/env python3
"""Compile and run one deterministic shard of an examples repository."""

from __future__ import annotations

import argparse
import json
import os
import re
import subprocess
import tempfile
import time
import xml.etree.ElementTree as ET
from dataclasses import asdict, dataclass
from datetime import datetime, timezone
from pathlib import Path
from typing import Any


SCHEMA_VERSION = 1
DEFAULT_ASPOSE_CAD_NUGET_VERSION = "26.9.0"
_FILE_LITERAL_RE = re.compile(r'"([^"\r\n]+\.([A-Za-z0-9]{2,5}))"')
_PLACEHOLDER_CONTENT = {
    "csv": b"col1,col2\nstub,stub\n",
    "docx": b"",
    "eml": b"Subject: Stub\r\nFrom: sender@example.com\r\n\r\nBody",
    "htm": b"<html><body>stub</body></html>",
    "html": b"<html><body>stub</body></html>",
    "ics": b"BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR\r\n",
    "json": b"{}",
    "mbox": b"From sender@example.com Thu Jan 01 00:00:00 2026\nSubject: Stub\n\nBody\n",
    "mht": b"From: stub\r\nContent-Type: text/html\r\n\r\n<body>stub</body>",
    "mhtml": b"From: stub\r\nContent-Type: text/html\r\n\r\n<body>stub</body>",
    "msg": b"",
    "oft": b"",
    "ost": b"",
    "pdf": b"",
    "pst": b"",
    "tgz": b"",
    "txt": b"stub\n",
    "vcf": b"BEGIN:VCARD\r\nVERSION:3.0\r\nFN:Stub\r\nEND:VCARD\r\n",
    "xlsx": b"",
    "xml": b"<root />\n",
    "xps": b"",
    "zip": b"",
}


@dataclass(frozen=True)
class ValidationResult:
    path: str
    category: str
    status: str
    stage: str
    compile_status: str
    runtime_status: str
    duration_ms: int
    error: str | None = None


def utc_now() -> str:
    return datetime.now(timezone.utc).isoformat()


def load_index(repo: Path) -> dict[str, Any]:
    path = repo / "index.json"
    if not path.exists():
        raise ValueError(f"Missing repository index: {path}")
    payload = json.loads(path.read_text(encoding="utf-8"))
    if not isinstance(payload, dict):
        raise ValueError("index.json must contain an object")
    return payload


def discover_examples(repo: Path) -> list[Path]:
    examples = []
    for path in repo.rglob("*.cs"):
        relative = path.relative_to(repo)
        if any(part.startswith(".") for part in relative.parts):
            continue
        examples.append(relative)
    return sorted(examples, key=lambda path: path.as_posix().lower())


def select_shard(
    examples: list[Path],
    *,
    shard_count: int,
    shard_index: int,
) -> list[Path]:
    if shard_count < 1:
        raise ValueError("shard_count must be at least 1")
    if not 0 <= shard_index < shard_count:
        raise ValueError("shard_index must be between 0 and shard_count - 1")
    return [
        path
        for index, path in enumerate(examples)
        if index % shard_count == shard_index
    ]


def _run(
    command: list[str],
    *,
    cwd: Path,
    timeout: int,
    env: dict[str, str],
    input_text: str | None = None,
) -> subprocess.CompletedProcess[str]:
    return subprocess.run(
        command,
        cwd=cwd,
        capture_output=True,
        text=True,
        timeout=timeout,
        env=env,
        input=input_text,
        check=False,
    )


def _short_error(output: str) -> str:
    lines = [line.strip() for line in output.splitlines() if line.strip()]
    for marker in ("error CS", "Unhandled exception", "error NU", "timed out"):
        for line in lines:
            if marker.lower() in line.lower():
                return line[:1000]
    return lines[0][:1000] if lines else "Unknown validation failure"


def _ensure_placeholders(code: str, workspace: Path) -> None:
    for filename, extension in _FILE_LITERAL_RE.findall(code):
        content = _PLACEHOLDER_CONTENT.get(extension.lower())
        if content is None:
            continue
        relative = Path(filename.replace("\\", "/"))
        if relative.is_absolute() or ".." in relative.parts:
            continue
        target = workspace / relative
        if not target.exists():
            target.parent.mkdir(parents=True, exist_ok=True)
            target.write_bytes(content)


def _write_project(
    project_dir: Path,
    *,
    cad_version: str,
) -> None:
    project = f"""\
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Aspose.CAD" Version="{cad_version}" />
  </ItemGroup>
</Project>
"""
    (project_dir / "CorpusValidation.csproj").write_text(
        project,
        encoding="utf-8",
    )
    (project_dir / "Program.cs").write_text(
        "internal static class Program { private static void Main() {} }\n",
        encoding="utf-8",
    )


def _validate_example(
    source: Path,
    relative: Path,
    *,
    project_dir: Path,
    case_root: Path,
    env: dict[str, str],
    build_timeout: int,
    run_timeout: int,
) -> ValidationResult:
    started = time.monotonic()
    code = source.read_text(encoding="utf-8", errors="replace")
    (project_dir / "Program.cs").write_text(code, encoding="utf-8")
    case_dir = case_root / relative.with_suffix("")
    case_dir.mkdir(parents=True, exist_ok=True)
    _ensure_placeholders(code, case_dir)

    try:
        build = _run(
            [
                "dotnet",
                "build",
                "CorpusValidation.csproj",
                "--configuration",
                "Release",
                "--no-restore",
                "--nologo",
                "--verbosity",
                "minimal",
            ],
            cwd=project_dir,
            timeout=build_timeout,
            env=env,
        )
    except subprocess.TimeoutExpired:
        return ValidationResult(
            path=relative.as_posix(),
            category=relative.parts[0],
            status="failed",
            stage="compile_timeout",
            compile_status="timeout",
            runtime_status="not_run",
            duration_ms=int((time.monotonic() - started) * 1000),
            error=f"dotnet build timed out after {build_timeout} seconds",
        )

    if build.returncode != 0:
        output = f"{build.stdout}\n{build.stderr}"
        return ValidationResult(
            path=relative.as_posix(),
            category=relative.parts[0],
            status="failed",
            stage="compile",
            compile_status="failed",
            runtime_status="not_run",
            duration_ms=int((time.monotonic() - started) * 1000),
            error=_short_error(output),
        )

    dll = project_dir / "bin" / "Release" / "net8.0" / "CorpusValidation.dll"
    try:
        runtime = _run(
            ["dotnet", str(dll)],
            cwd=case_dir,
            timeout=run_timeout,
            env=env,
            input_text="\n",
        )
    except subprocess.TimeoutExpired:
        return ValidationResult(
            path=relative.as_posix(),
            category=relative.parts[0],
            status="failed",
            stage="runtime_timeout",
            compile_status="passed",
            runtime_status="timeout",
            duration_ms=int((time.monotonic() - started) * 1000),
            error=f"dotnet run timed out after {run_timeout} seconds",
        )

    if runtime.returncode != 0:
        output = f"{runtime.stdout}\n{runtime.stderr}"
        return ValidationResult(
            path=relative.as_posix(),
            category=relative.parts[0],
            status="failed",
            stage="runtime",
            compile_status="passed",
            runtime_status="failed",
            duration_ms=int((time.monotonic() - started) * 1000),
            error=_short_error(output),
        )

    return ValidationResult(
        path=relative.as_posix(),
        category=relative.parts[0],
        status="passed",
        stage="completed",
        compile_status="passed",
        runtime_status="passed",
        duration_ms=int((time.monotonic() - started) * 1000),
    )


def _git_sha(repo: Path) -> str | None:
    result = subprocess.run(
        ["git", "rev-parse", "HEAD"],
        cwd=repo,
        capture_output=True,
        text=True,
        check=False,
    )
    return result.stdout.strip() if result.returncode == 0 else None


def write_junit(
    path: Path,
    *,
    results: list[ValidationResult],
    shard_index: int,
) -> None:
    failures = sum(result.status != "passed" for result in results)
    suite = ET.Element(
        "testsuite",
        {
            "name": f"full-corpus-shard-{shard_index}",
            "tests": str(len(results)),
            "failures": str(failures),
            "errors": "0",
            "time": str(
                round(sum(result.duration_ms for result in results) / 1000, 3)
            ),
        },
    )
    for result in results:
        case = ET.SubElement(
            suite,
            "testcase",
            {
                "classname": result.category,
                "name": result.path,
                "time": str(round(result.duration_ms / 1000, 3)),
            },
        )
        if result.status != "passed":
            failure = ET.SubElement(
                case,
                "failure",
                {
                    "type": result.stage,
                    "message": result.error or result.stage,
                },
            )
            failure.text = result.error or result.stage
    path.parent.mkdir(parents=True, exist_ok=True)
    ET.ElementTree(suite).write(path, encoding="utf-8", xml_declaration=True)


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser()
    parser.add_argument("--repo", required=True, type=Path)
    parser.add_argument("--out", required=True, type=Path)
    parser.add_argument("--junit", required=True, type=Path)
    parser.add_argument("--shards", type=int, default=1)
    parser.add_argument("--shard", type=int, default=0)
    parser.add_argument("--build-timeout", type=int, default=120)
    parser.add_argument("--run-timeout", type=int, default=30)
    parser.add_argument("--limit", type=int)
    parser.add_argument("--cad-version")
    return parser.parse_args()


def resolve_cad_version(index: dict[str, Any], override: str | None = None) -> str:
    version = (
        override
        or os.getenv("ASPOSE_CAD_NUGET_VERSION")
        or DEFAULT_ASPOSE_CAD_NUGET_VERSION
        or str(index.get("package_version") or "").strip()
    )
    version = version.strip()
    if not version:
        raise ValueError("No Aspose.CAD package version is configured")
    return version


def main() -> int:
    args = parse_args()
    repo = args.repo.resolve()
    started_at = utc_now()
    index = load_index(repo)
    examples = discover_examples(repo)
    expected_total = int(index.get("total_examples") or 0)
    if expected_total and expected_total != len(examples):
        raise ValueError(
            f"index.json reports {expected_total} examples but discovered "
            f"{len(examples)}"
        )
    selected = select_shard(
        examples,
        shard_count=args.shards,
        shard_index=args.shard,
    )
    if args.limit is not None:
        selected = selected[: max(0, args.limit)]

    cad_version = resolve_cad_version(index, args.cad_version)
    env = dict(os.environ)
    env.update(
        {
            "DOTNET_CLI_TELEMETRY_OPTOUT": "1",
            "DOTNET_NOLOGO": "1",
            "DOTNET_SKIP_FIRST_TIME_EXPERIENCE": "1",
        }
    )

    results: list[ValidationResult] = []
    with tempfile.TemporaryDirectory(prefix="cad_corpus_validation_") as tmp:
        root = Path(tmp)
        project_dir = root / "project"
        case_root = root / "cases"
        project_dir.mkdir()
        case_root.mkdir()
        _write_project(
            project_dir,
            cad_version=cad_version,
        )
        restore = _run(
            ["dotnet", "restore", "CorpusValidation.csproj", "--nologo"],
            cwd=project_dir,
            timeout=600,
            env=env,
        )
        if restore.returncode != 0:
            raise RuntimeError(
                f"dotnet restore failed: "
                f"{_short_error(restore.stdout + restore.stderr)}"
            )

        for position, relative in enumerate(selected, start=1):
            result = _validate_example(
                repo / relative,
                relative,
                project_dir=project_dir,
                case_root=case_root,
                env=env,
                build_timeout=args.build_timeout,
                run_timeout=args.run_timeout,
            )
            results.append(result)
            if position % 25 == 0 or position == len(selected):
                passed = sum(item.status == "passed" for item in results)
                print(
                    f"[{position}/{len(selected)}] "
                    f"passed={passed} failed={position - passed}"
                )

    passed = sum(result.status == "passed" for result in results)
    failed = len(results) - passed
    payload = {
        "schema_version": SCHEMA_VERSION,
        "validation_id": os.getenv("CI_PIPELINE_ID") or f"local-{int(time.time())}",
        "repository": os.getenv(
            "EXAMPLES_REPO_URL",
            "https://github.com/aspose-cad/agentic-net-examples.git",
        ),
        "commit_sha": _git_sha(repo),
        "package_version": cad_version,
        "started_at": started_at,
        "finished_at": utc_now(),
        "shard_count": args.shards,
        "shard_index": args.shard,
        "discovered_total": len(examples),
        "selected_total": len(selected),
        "compile_passed": sum(
            result.compile_status == "passed" for result in results
        ),
        "compile_failed": sum(
            result.compile_status in {"failed", "timeout"} for result in results
        ),
        "runtime_passed": sum(
            result.runtime_status == "passed" for result in results
        ),
        "runtime_failed": sum(
            result.runtime_status in {"failed", "timeout"} for result in results
        ),
        "passed": passed,
        "failed": failed,
        "pass_rate": round(passed / len(results), 6) if results else 0.0,
        "results": [asdict(result) for result in results],
    }
    args.out.parent.mkdir(parents=True, exist_ok=True)
    args.out.write_text(
        json.dumps(payload, indent=2, sort_keys=True) + "\n",
        encoding="utf-8",
    )
    write_junit(args.junit, results=results, shard_index=args.shard)
    print(
        f"Shard {args.shard + 1}/{args.shards}: "
        f"passed={passed} failed={failed} report={args.out}"
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
