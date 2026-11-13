# Publication and repair validation

## Publication counts

This branch proposes **1,906 examples across 45 categories** from the 1,959-task catalog; **53 tasks are withheld**. Relative to merged main's 1,905 examples, it replaces 45 existing sources and adds owner-clarified visible-watermark task **45026**. No previously published source is removed.

The earlier checkpoints were 1,864 + 60 added IDs - 30 withheld IDs = 1,894, followed by 45757 to 1,895, eight native watermark examples to 1,903 and DGN comparison 44122 to 1,904. The SAT reader brought the inventory to **1,904 + 1 = 1,905**. The November 12 owner-clarified visible annotation adds one further example: **1,905 + 1 = 1,906**. Publication counts differ from historical catalog-run statistics.

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


## November 6 rerun repairs

The November 5–6 generation run ended with 23 failures. This branch replaces 22
existing sources; it adds no examples and removes no previously published source.
The inventory remains 1,905 examples across 45 categories, with 54 tasks withheld.
The original run summary is preserved; repaired local evidence is separate.

All 22 exact sources compiled and executed against a controlled native DWG/DXF and
PNG input on Aspose.CAD 26.9.0 / .NET 8. Native-save samples reload their outputs
and assert entity membership, references, ownership, widths or hatch data. Render
samples check callback completion/PDF signatures or TIFF dimensions. Thirteen
controls exercise missing/corrupt inputs, absent entities, unsupported overlapping
arcs and block-only polylines. Independent geometry checks verify the identify-only
and deletion distinction, preserve an AABB-overlapping line that misses the
rectangle, and confirm rectangle-fill replacement preserves outlines.

Collection semantics are explicit: definitions are CadBaseObject entries in
Objects; raster instances are CadEntityBase entries in Entities. Task 45240 sets
missing-text glyph fallback, not a font on hatch line geometry. Task 45184 handles
linked raster definitions, not all DWG XREF/underlay formats. Rectangle operations
support exact XY lines, circles and straight coplanar lightweight polylines;
unsupported overlapping geometry fails before save instead of deleting by bounds.

Task 44046 remains withheld: a fresh PDF probe rendered successfully but embedding
stayed PENDING and CAD could not reload the PDF for known-payload validation.
The paired native DXF control passed correct-payload and wrong-payload checks;
that is not PDF watermark proof.

Full-corpus Linux CI for this new source commit is pending. Earlier corpus CI
belongs to earlier commits; most corpus runtime cases are missing-input smoke
checks, distinct from the representative drawing evidence for these 22 repairs.

Tracking: https://issue.support.dynabic.com/issues/137635


## November 12 stopped-run repairs

This initial 12/13 checkpoint preceded the owner clarification below. Its 1,905/54 inventory and withheld status for 45026 are historical; the [visible annotation follow-up](#visible-watermark-scope-clarification--november-12) supersedes them with 1,906/53.

Stopped run 20261111_174822_766873_5aebc600 records 13 failures. Its aggregate
counters show 780 started / 766 successes / 13 failures, while the per-task table
contains 767 passes / 13 failures. These historical records are preserved and
are not represented as a completed 1,959-task run.

Twelve repaired/restored sources pass exact-source compile/run with native
DWG/DXF or public DWF input. Seventeen input/output controls pass, including
paper-space placement, curved-outline rejection, missing batch/donor inputs,
DWG donors, unsupported import dependencies, hidden layers and PDF size limits.
Four further assertions on the compiled callback/layer helpers reject incomplete
renders and reported failures and verify the actual visibility/exclusion policy.
Original input hashes remain unchanged.

The twelve include three unchanged sources restored from existing verified
examples (45056, 45328 and 45986) and nine newly changed source files. Together
with the earlier branch's 22 changes, this review now updates 31 source files,
not 34 additions. All paths/discovery counts remain unchanged: 1,905 examples
and 54 withheld tasks.

Polyline insertion verifies geometry, layer, color and selected model/paper
layout ownership after native reload. Native serialization can expand omitted
bulges to explicit zeros; comparisons preserve that logical equivalence while
retaining genuine curved values. Hatch samples retain actual line-family data
and reject unsupported curved outlines. Composite import preserves the existing
target and supports bounded LINE/CIRCLE/TEXT dependencies, not arbitrary models.
DWF font mapping checks actual typed font names and retained text/positions;
host font installation and general glyph appearance are not universally certified.
The configured high-quality PDF/A output has part 1/conformance A XMP and is
checked against a caller-specified size limit; independent conformance validation
is not claimed.

Task 45026 remains withheld: native DXF payload validation and a PDF margin
preview do not verify an invisible watermark's alleged visible border placement.
A reused image reports SUCCESS after native DXF then PDF, while fresh-image PDF
export reports PENDING in the diagnostic. Status on a reused image is not proof
of payload persistence in the exact PDF. No overlay or native-DXF substitution
is counted as the missing operation.

Current-source Linux full-corpus CI is pending for this expanded branch. Earlier
CI belongs to its earlier source commit. Most corpus runtime cases remain
missing-input smoke checks, separate from the native drawing checks above.

Tracking: https://issue.support.dynabic.com/issues/137921
Generator review: https://gitlab.recruitize.ai/sialkot/gujrat-examples/aspose-cad-examples-generator-v1/-/merge_requests/16


## Visible watermark scope clarification — November 12

After the initial 12/13 repair checkpoint, the owner explicitly requested **visible watermark text inside the border** for task 45026. Preserve the original blind-watermark failure/diagnostic as history; the new source, title, path, manifest and discovery metadata explicitly describe visible annotation. This does not establish PDF blind-watermark embedding or repair the separate unresolved protection tasks.

The sample appends a closed border and a CadText in a reserved caption strip around the finite native drawing extents. It registers both entities in model space without moving the original drawing, saves DXF in Convert mode, reloads to verify text, coordinates, ownership, original entity membership and layouts, and exports the reloaded drawing with margins and render callback checks. Existing outputs are rejected; captions are bounded to 1–40 printable ASCII characters.

Three final-source drawing executions pass: DWG with the default caption, DXF and DWG with a longer caption. Independent PDFKit text extraction and rasterization plus pixel checks locate the red caption strictly inside the blue border, with the border strictly inside the page. A separate native DXF comparison preserves geometry groups, text, layer, flags, handles and owner on all ten original entities. Nine final-source controls pass (two drawing positives, six rejection cases and a no-input smoke check). Evaluation-mode decorations remain visible and are excluded from the color-based placement measurement. Broader font, encoding, layout and drawing coverage is not certified.

Inventory progression: **1,905 + 1 = 1,906 examples**, **54 - 1 = 53 withheld**. The combined review replaces 31 existing C# sources and adds this one example; it does not add 32 examples. Current-source corpus CI must be read from the PR checks; the earlier 1,905-source CI applies to its earlier commit. Most corpus executions are missing-input checks, separate from these three real-input runs.

Generator regressions: 349 focused passes; full required suite 691 passed, 43 unchanged inherited failures, 136 passing subtests, 84.76% coverage. Source/control/visual evidence and metrics receipts are linked from the PR and task #137921.


## November 13 failure repairs

The interrupted November 12–13 run recorded 18 failures after 779 cached tasks and 832 new starts: 1,592 successes including cache, 18 failures, with the original run still marked running when its owner exited. The process and local API are no longer running. Original run/failure records are retained; this is not a completed 1,959-task pass. The orphaned usage state was recovered through the existing metrics API path: 4,474 LLM calls / 16,997,662 tokens / 949 MCP generation calls / 5,423 total API calls, acknowledgment recorded. API recovery status follows its existing interrupted-run reporting policy and does not assert full catalog completion.

Fourteen current exact standalone sources pass static validation, restore, compile and real-file runtime checks. All 25 final-source controls pass, including application callback failure, color range, model/paper layout references, unknown layout, actual transient HTTP retry, exhaustion/non-transient/corrupt responses, wrong models and native SAT deletion rejection controls. Three additional assertions against the exact compiled reference method verify cyclic traversal/underlay resolution/unlinked exclusion, missing definition rejection and ambiguous paper ownership rejection. Inputs remain unchanged.

Quality export sets the real text/hatch/arc quality group and verifies PDF callbacks without changing drawing geometry. Layer-selection measurement temporarily assigns the selected array, checks the other geometry and layer definitions and restores the original collection in finally. Native DwgImage.AddLayer is required to register generated layers. Only AC1032 DWG output is verified in this SDK; AC1027 and AC1018 writer calls are unimplemented and are explicitly rejected before final output. The controlled 3DFACE retains zero-Z first/second corners and nonzero-Z third/fourth corners; arbitrary face orientations are not certified.

Block/insert examples preserve the small fixture's original geometry, handles and layouts on native reload. A larger capacity fixture loses entities through this SDK writer in the current environment; all three preservation controls reject it and publish no final DWG. The examples support bounded geometry/dependencies, not arbitrary large drawings. Pending files remain distinguishable from verified output. The existing DXF-to-DWG importer retains LINE/CIRCLE/TEXT dependency checks.

The external-reference logger follows actual layout ownership, including owner-less DXF model semantics, through insert blocks with cycle detection. Raster and underlay paths resolve through real definition handles. Model and paper references remain separated; unused definitions are excluded. DRC retry notifications are application callbacks around real HTTP failures, not invented SDK load callbacks. DWFx PNG and DWF JPEG checks inspect actual exported dimensions; DWF layer names and JPEG bytes are recorded.

The SAT task operates on native CAD containing text ACIS/SAT data. Its explicitly labelled application RemoveEntityAt helper uses actual SDK removal/collections. Reload verifies one deletion, the remaining ten entity identities/ACIS payloads, viewport geometry and layout ownership. Standalone SAT/SAB and unsupported preservation comparisons are rejected.

Four tasks remain unverified: 44331 (FBX batch to protected PDF), 44332 (IFC batch to protected PNG), 44345/44351 (IFC line pattern/default line type). Fresh FBX/IFC embedding remains PENDING; reloaded PNG payload validation is FAILURE; the SDK cannot reload the diagnostic PDFs. A fresh 36-export IFC pixel probe has real geometry/color controls but no curve-font pattern effect. Successful rendering, status queries or alternate operations are not counted as their repair.

Full required generator suite: 704 passed / 43 unchanged inherited failures / 136 passing subtests, 84.81% coverage; full suite is not green. Generator MR17 and task138019 carry source/control/fixture/usage evidence. This review cumulatively replaces **45 existing sources and adds 1** (the earlier owner-clarified visible annotation), with inventory unchanged at **1,906 / 53 withheld**. Current-source corpus CI must be read from PR checks; prior source results do not cover these changes.

Tracking: https://issue.support.dynabic.com/issues/138019
Generator review: https://gitlab.recruitize.ai/sialkot/gujrat-examples/aspose-cad-examples-generator-v1/-/merge_requests/17
