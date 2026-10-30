# Publication and repair validation

## Publication counts

This branch publishes **1,895 examples across 45 categories** from the 1,959-task catalog; **64 tasks are withheld**. Relative to the merged October 29 publication on main, this change adds one verified example: task **45757**. No previously published example is removed.

The earlier October 29 release reconciled its count as 1,864 + 60 added task IDs - 30 withheld IDs = 1,894. The October 30 recovery brings that total to **1,895**. These publication inventories differ from historical catalog-run statistics.

The original completed run processed 1,959 tasks with 1,920 successes and 39 failures. The subsequent repair batches recovered 34 on October 29 and one on October 30: **35 recoveries, four unresolved**. The original run history is unchanged.

## Existing DWG insertion repair (45757)

The standalone sample transfers the existing DWG's public CAD state into a native `DwgImage` writer, keeping the source alive until saving finishes. It copies the header, objects/layouts, entities, block records/definitions, layers, styles and other drawing tables. It uses no reflection or private API. A loaded `CadImage` must not be cast to `DwgImage`, and copying only entities is insufficient to preserve the drawing.

Native `AddBlock` / `AddEntityToBlock` register the new definition; a `CadInsertObject` references it. The sample saves with `DwgOptions { OutputMode = CadOutputMode.Convert }`, reloads the pending DWG, and checks original geometry, handles, layout/block ownership, imported geometry, placement and scale before publishing the output filename.

Exact-source tests preserved ten existing entities and three layouts in the primary drawing. A second control preserved twelve entities, including paper-space geometry and an existing rotated/scaled insert. Additional controls reject unsupported external geometry without a successful output and handle a missing external file without claiming a completed insertion. Original input-file hashes remain unchanged.

The bounded external importer supports LINE, CIRCLE and TEXT, with checks for layer, style, linetype and extended-data dependencies. Existing geometry verification also supports ARC, SOLID, MTEXT, lightweight polylines and INSERT. Unsupported entity types/dependencies are rejected; this is not a universal dependency-aware importer.

## Remaining watermark tasks

| Task IDs | Evidence gap |
| --- | --- |
| 44046, 44056, 44637, 44638 | PDF/PNG blind-watermark embedding remains PENDING after export in the pinned SDK; PNG reload validation fails. |

A 24-case local matrix checked DWG/DXF/OBJ inputs, PDF/PNG/DXF/3D-PDF outputs, and Render/Convert modes. Changing modes or enabling 3D PDF did not resolve the requested PDF/PNG protection. The rendered PDFs were not loadable by `Image.Load` in these tests. A DXF Convert control embeds and validates the expected payload successfully, so this is not a blanket failure of blind watermarking.

These four cases remain excluded. A visible overlay, embedded metadata, a DXF substitute, or a completed render is not reported as successful blind-watermark protection. They are a subset of the 64 withheld catalog tasks.

## Corpus validation

The merged 1,894-example corpus passed [Linux run 40895028436](https://github.com/aspose-cad/agentic-net-examples/actions/runs/40895028436). The current 1,895-example branch requires its own CI run. The new source has separate drawing-input compile/run and preservation/negative-control evidence.

Most corpus executions check missing-input handling; their success does not establish drawing-specific functional correctness for the entire repository. Earlier raster/hatch/text persistence checks use editable DXF output. PDF/A export configuration is not independent conformance certification, and preview generation is not a visual quality review.

The earlier 44008 entrypoint repair keeps a safe missing-input exit while real conversion errors still fail; a real DXF-to-PDF conversion also succeeded. Its original audit failure remains in the generator evidence.

## Workflow and reproduction

The existing workflow runs on pull requests, main pushes, manual dispatches and weekly schedules. Eight shards compile and execute the full corpus. The aggregate gate requires 100% success and complete shard coverage. The manifest rejects changed, missing or additional unverified sources; JSON, JUnit, Markdown and HTML artifacts are retained.

```bash
python .github/scripts/check_manifest.py --repo .
python .github/scripts/validate_example_corpus.py --repo . --shards 1 --shard 0 \
  --cad-version 26.9.0 --out validation/shard-0.json --junit validation/junit-0.xml
```

Keep .NET 8 and the pinned Aspose.CAD 26.9.0 package when reproducing this evidence. Framework dependencies are restored by NuGet. No SDK DLLs, credentials, licenses, generated build workspaces or customer drawings are committed.
