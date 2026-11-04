# Publication and repair validation

## Publication counts

This branch proposes **1,905 examples across 45 categories** from the 1,959-task catalog; **54 tasks are withheld**. Relative to merged main's 1,904 examples, it adds the native SAT-container layout reader **45173**. No previously published source is removed.

The earlier checkpoints were 1,864 + 60 added IDs - 30 withheld IDs = 1,894, followed by 45757 to 1,895, eight native watermark examples to 1,903 and DGN comparison 44122 to 1,904. This branch adds one: **1,904 + 1 = 1,905**. Publication counts differ from historical catalog-run statistics.

The original completed run processed 1,959 tasks with 1,920 successes and 39 failures. Subsequent repair batches recovered 35 of those failures; four PDF/PNG watermark operations remain unresolved. The prior nine repairs and this SAT-container reader come from the wider withheld inventory and do not change that 35/39 figure.

## SAT-container layouts (45173) — November 4

The sample loads a native DWG/DXF as `CadImage`, checks for non-empty text ACIS/SAT records on actual `CadObjectWithAcisData` objects (including block definitions), and reads layout names from that same drawing. It does not cast a standalone SAT image to `CadImage` or manufacture layout names. The official public `conic_pyramid.dxf` fixture exposes nine ACIS objects and the actual `Model` and `Layout1` records. Text groups can be obfuscated; the sample checks their presence without decoding or exporting SAT. Binary-only ACIS and mesh data are explicitly outside scope. This is not SAT kernel/solid-validity certification.

The exact source compiles and runs on .NET 8/Aspose.CAD 26.9. Eight checks pass: default and relative-path positive invocations; five rejection cases (native input without SAT, a 3D mesh without SAT, a non-native model, an explicit missing input and invalid arguments); and a no-input smoke check. All source/fixture hashes are retained, and original inputs remain byte-identical. The sample never saves or reconstructs the drawing.

The presence check follows the public [DXF 3DSOLID proprietary-data groups](https://help.autodesk.com/cloudhelp/2020/ENU/AutoCAD-DXF/files/GUID-19AB1C40-0BE0-4F32-BCAB-04B37044A0D3.htm). Positive fixture coverage is a native DXF containing text ACIS/SAT; it does not establish standalone SAT layouts or arbitrary binary DWG kernel access.

## Native watermark workflows — November 4

The eight exact sources compile and run with controlled DWG/DXF drawings. They verify the owner's known expected text payload and reject an incorrect one. Embedding saves a pending DXF with Convert mode, checks embedding after Save, reloads and validates the actual saved file, preserves drawing entity counts/layout names and source input hashes, then moves the verified file to its final name. Watermark encoding can change geometry; bit-identical output geometry is not claimed.

Presence-check tasks require the known expected payload; there is no arbitrary-watermark detector. Multi-format coverage is bounded to native DWG/DXF inputs and verified DXF output. The asset-publication sample produces a verified local artifact directory, not an external Git upload. Five failure controls reject absent payloads, wrong native models and low-capacity drawings without publishing output or a readiness manifest. All eight positive cases and all five controls pass.

## DGN underlay comparison (44122) — November 4

The sample follows each `CadDgnUnderlay.AcDbUnderlayDefinitionObjectId` to its `CadDgnDefinition` in `Objects`, normalizes Windows path separators and resolves relative paths from the DWG directory. It loads referenced and reference files as `DgnImage`, compares SDK-visible entity-type counts and versions, and reports full-file SHA-256 byte equality. Equal counts do not establish geometric equivalence; evaluation mode can truncate exposed entities. This is external-underlay comparison, not extraction of embedded DGN geometry. It does not modify or save inputs.

The exact source passed 11 checks: two drawing-input comparisons (matching and different DGN files, including a relative path from another working directory), seven missing/model/link rejection cases, one no-input smoke check, and one explicit SDK loading-error control. Fixtures include the official public `BlockRefDgn.dwg` and camera/seed DGN files. The underlay's referenced filename is populated with the official DGN fixture for the test; original fixture hashes are retained. All input hashes remain unchanged.

A separate SDK-generated DWG underlay fixture loads when its external DGN is absent but raises `ImageLoadException`/`NullReferenceException` in native bounds calculation when the DGN is present. Changing model name, initializing flags/clipping/extrusion, or loading from a stream did not resolve that case. The sample returns a failure for it; it does not enable `IgnoreErrors`, hide references, or claim this SDK loading limitation is repaired. Successful comparison is bounded to loadable DWGs and SDK-exposed entity data. The positive drawing-input tests ran on macOS; corpus Linux checks separately exercise missing-input handling.

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

These four cases remain excluded. A visible overlay, embedded metadata, a DXF substitute, or a completed render is not reported as successful blind-watermark protection. They are a subset of the 54 withheld catalog tasks in this candidate.

## Corpus validation

Merged main's 1,904-example baseline passed [run 41909423202](https://github.com/aspose-cad/agentic-net-examples/actions/runs/41909423202). The expanded 1,905-source candidate has its own full-corpus PR checks; inspect the current source commit. Task 45173 has separate drawing-input checks and rejection controls. Prior native watermark, DGN comparison and 45757 drawing-preservation evidence is retained.

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
