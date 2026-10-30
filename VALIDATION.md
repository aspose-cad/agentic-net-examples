# Publication and repair validation

## Publication counts

The checked-in validation manifest is the authoritative publication inventory. This branch contains **1,894 examples in 45 categories** from the 1,959-task catalog; **65 catalog tasks are withheld**.

Relative to the current `main` manifest (1,864 examples), this publication adds 60 task IDs, removes 30 task IDs whose requested operation remains unverified, and changes the source hashes of 47 retained examples. The resulting count is **1,864 + 60 - 30 = 1,894**. These are accumulated publication changes, not 60 new repairs in the latest batch.

The October 29 repair batch recovered **34 of the 39 failures** from the completed full-catalog run. Within the prior pending publication, it updated 33 examples, added task 44054, and removed two earlier candidates (44638 and 45757) after drawing-input audits failed to verify their requested operation.

## Verification evidence

- The exact 34 repaired sources compiled and ran against .NET 8 and Aspose.CAD 26.9.0 with controlled drawing inputs, operation assertions, and output checks. Eight additional controls checked negative cases, deletion counts, and JPEG resolution metadata.
- The earlier C# corpus passed [Linux validation run 40889126152](https://github.com/aspose-cad/agentic-net-examples/actions/runs/40889126152): **1,894 compiled and executed, zero failures**, across eight shards and the aggregate gate. That run tested the earlier branch's merge commit `30fe61d7c0cbe8d38cd281a192a2bbc66ef6ac0c`; the new PR receives a separate CI run. A subsequent whitespace-only cleanup in the 3D polyline sample leaves its C# tokens unchanged and was recompiled and executed locally; its manifest hash was refreshed.
- Most corpus executions check missing-input handling. They do not establish drawing-specific functional correctness for all 1,894 examples. Supply representative input files before relying on an exported result.
- Raster, hatch, and text persistence in the repaired examples was verified using editable DXF output. This is not evidence of unrestricted native DWG editing. PDF/A configuration and export were checked; independent PDF/A certification and visual quality review are separate checks.
- `validation-manifest.json` records source hashes and verification notes. CI rejects missing, extra, changed, or unverified source files.

## Unresolved tasks

| Task IDs | Remaining evidence gap |
| --- | --- |
| 44046, 44056, 44637, 44638 | PDF/PNG blind watermark checks remain PENDING after export. |
| 45757 | The newly inserted block does not survive native DWG reload in the tested workflow. |

These five tasks remain excluded from publication. They are a subset of the 65 withheld catalog tasks, not an additional 65 failures.

## Workflow and reproducibility

The existing workflow runs on pull requests, main pushes, manual dispatches, and weekly schedules. Eight shards compile and run the full published corpus; the aggregate gate requires 100% success and complete shard coverage. JSON, JUnit, Markdown, and HTML results are retained as workflow artifacts.

The earlier task 44008 entrypoint repair keeps a safe missing-input exit while conversion errors still fail; a real DXF-to-PDF conversion also succeeded. Its original audit failure remains in the generator evidence.

The only direct validation dependency is the pinned Aspose.CAD NuGet package; framework dependencies are restored by NuGet. No SDK DLLs, credentials, license files, generated build workspaces, or customer drawings are committed.

## Reproduce the manifest check

```bash
python .github/scripts/check_manifest.py --repo .
python .github/scripts/validate_example_corpus.py --repo . --shards 1 --shard 0 \
  --cad-version 26.9.0 --out validation/shard-0.json --junit validation/junit-0.xml
```

Run the existing pull-request workflow for the full compile-and-run corpus check. Keep the pinned SDK version when reproducing the recorded evidence.
