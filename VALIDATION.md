# Publication and repair validation

## Publication counts

This branch proposes **1,903 examples across 45 categories** from the 1,959-task catalog; **56 tasks are withheld**. Relative to merged main's 1,895 examples, it adds eight native watermark workflows: **44059, 44062, 44065, 44066, 44067, 44068, 44069 and 44796**. No previously published source is removed.

The earlier checkpoints were 1,864 + 60 added IDs - 30 withheld IDs = 1,894, followed by the 45757 recovery to 1,895. This batch adds eight: **1,895 + 8 = 1,903**. Publication counts differ from historical catalog-run statistics.

The original completed run processed 1,959 tasks with 1,920 successes and 39 failures. Subsequent repair batches recovered 35 of those failures; four PDF/PNG watermark operations remain unresolved. The eight additions here come from the wider withheld inventory and do not change that 35/39 figure.

## Native watermark workflows — November 4

The eight exact sources compile and run with controlled DWG/DXF drawings. They verify the owner's known expected text payload and reject an incorrect one. Embedding saves a pending DXF with Convert mode, checks embedding after Save, reloads and validates the actual saved file, preserves drawing entity counts/layout names and source input hashes, then moves the verified file to its final name. Watermark encoding can change geometry; bit-identical output geometry is not claimed.

Presence-check tasks require the known expected payload; there is no arbitrary-watermark detector. Multi-format coverage is bounded to native DWG/DXF inputs and verified DXF output. The asset-publication sample produces a verified local artifact directory, not an external Git upload. Five failure controls reject absent payloads, wrong native models and low-capacity drawings without publishing output or a readiness manifest. All eight positive cases and all five controls pass.

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

These four cases remain excluded. A visible overlay, embedded metadata, a DXF substitute, or a completed render is not reported as successful blind-watermark protection. They are a subset of the 56 withheld catalog tasks in this candidate.

## Corpus validation

The merged 1,895-example baseline passed [main run 41079861290](https://github.com/aspose-cad/agentic-net-examples/actions/runs/41079861290). The expanded 1,903-example candidate has separate full-corpus checks on its pull request; inspect the checks for the current source commit rather than reusing the baseline result. The eight added sources have separate exact-source drawing-input compile/run evidence and failure controls. The original 45757 drawing-preservation evidence is retained.

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
