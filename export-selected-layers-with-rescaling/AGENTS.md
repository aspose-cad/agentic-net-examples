---
name: export-selected-layers-with-rescaling
description: C# examples for export-selected-layers-with-rescaling using Aspose.CAD for .NET
language: csharp
framework: net8.0
parent: ../AGENTS.md
---

# AGENTS - export-selected-layers-with-rescaling

## Persona
You are a C# developer working in the **export-selected-layers-with-rescaling** category.
This folder contains standalone C# examples for this category.
See the root [AGENTS.md](../AGENTS.md) for repository-wide conventions.

## Scope
- Examples for **export-selected-layers-with-rescaling**.
- Files are standalone `.cs` examples stored directly in this folder.

## Required Namespaces
- `using System;` (29 file(s))
- `using Aspose.CAD;` (29 file(s))
- `using Aspose.CAD.FileFormats.Cad;` (28 file(s))
- `using System.IO;` (27 file(s))
- `using Aspose.CAD.ImageOptions;` (27 file(s))
- `using Aspose.CAD.FileFormats.Cad.CadTables;` (14 file(s))
- `using Aspose.CAD.FileFormats.Cad.CadObjects;` (14 file(s))
- `using System.Linq;` (12 file(s))
- `using System.Collections.Generic;` (7 file(s))

## Files in this folder
| File | Description |
|------|-------------|
| [after-export-read-the-output-file-metadata-to-confirm-that-only-selected-layers-are-present.cs](./after-export-read-the-output-file-metadata-to-confirm-that-only-selected-layers-are-present.cs) | after export read the output file metadata to confirm that only selected layers are present |
| [apply-updatesize-after-assigning-entities-to-guarantee-correct-scaling-before-export.cs](./apply-updatesize-after-assigning-entities-to-guarantee-correct-scaling-before-export.cs) | apply updatesize after assigning entities to guarantee correct scaling before export |
| [assign-the-cadbaseentity-array-to-the-drawing-entities-collection-before-export.cs](./assign-the-cadbaseentity-array-to-the-drawing-entities-collection-before-export.cs) | assign the cadbaseentity array to the drawing entities collection before export |
| [call-the-updatesize-method-to-apply-rescaling-to-the-selected-layers.cs](./call-the-updatesize-method-to-apply-rescaling-to-the-selected-layers.cs) | call the updatesize method to apply rescaling to the selected layers |
| [check-that-calling-updatesize-after-assigning-entities-does-not-affect-layers-not-in-the-cadbaseentity-array.cs](./check-that-calling-updatesize-after-assigning-entities-does-not-affect-layers-not-in-the-cadbaseentity-array.cs) | check that calling updatesize after assigning entities does not affect layers not in the cadbaseentity array |
| [compare-the-exported-layout-size-with-the-original-using-get-size-of-the-layout.cs](./compare-the-exported-layout-size-with-the-original-using-get-size-of-the-layout.cs) | compare the exported layout size with the original using get size of the layout |
| [configure-export-options-to-include-only-layers-present-in-the-cadbaseentity-array.cs](./configure-export-options-to-include-only-layers-present-in-the-cadbaseentity-array.cs) | configure export options to include only layers present in the cadbaseentity array |
| [create-a-cadbaseentity-array-instance-to-hold-the-selected-layer-objects.cs](./create-a-cadbaseentity-array-instance-to-hold-the-selected-layer-objects.cs) | create a cadbaseentity array instance to hold the selected layer objects |
| [create-multiple-cadbaseentity-arrays-for-different-layer-groups-and-export-each-separately.cs](./create-multiple-cadbaseentity-arrays-for-different-layer-groups-and-export-each-separately.cs) | create multiple cadbaseentity arrays for different layer groups and export each separately |
| [document-the-process-of-selecting-layers-updating-size-and-exporting-for-reproducibility.cs](./document-the-process-of-selecting-layers-updating-size-and-exporting-for-reproducibility.cs) | document the process of selecting layers updating size and exporting for reproducibility |
| [ensure-that-only-layers-added-to-the-cadbaseentity-array-are-included-in-the-export.cs](./ensure-that-only-layers-added-to-the-cadbaseentity-array-are-included-in-the-export.cs) | ensure that only layers added to the cadbaseentity array are included in the export |
| [ensure-that-the-export-function-respects-the-cadbaseentity-array-when-multiple-layouts-exist.cs](./ensure-that-the-export-function-respects-the-cadbaseentity-array-when-multiple-layouts-exist.cs) | ensure that the export function respects the cadbaseentity array when multiple layouts exist |
| [export-selected-layers-and-then-use-get-size-of-the-layout-to-compare-against-expected-dimensions.cs](./export-selected-layers-and-then-use-get-size-of-the-layout-to-compare-against-expected-dimensions.cs) | export selected layers and then use get size of the layout to compare against expected dimensions |
| [export-selected-layers-then-reopen-the-file-to-confirm-excluded-layers-are-absent.cs](./export-selected-layers-then-reopen-the-file-to-confirm-excluded-layers-are-absent.cs) | export selected layers then reopen the file to confirm excluded layers are absent |
| [get-size-of-the-layout-to-establish-baseline-dimensions-before-rescaling.cs](./get-size-of-the-layout-to-establish-baseline-dimensions-before-rescaling.cs) | get size of the layout to establish baseline dimensions before rescaling |
| [identify-the-layers-to-export-by-reviewing-the-drawing-layer-list.cs](./identify-the-layers-to-export-by-reviewing-the-drawing-layer-list.cs) | identify the layers to export by reviewing the drawing layer list |
| [invoke-the-export-function-to-generate-the-output-file-containing-selected-layers.cs](./invoke-the-export-function-to-generate-the-output-file-containing-selected-layers.cs) | invoke the export function to generate the output file containing selected layers |
| [iterate-through-all-layers-select-those-matching-a-naming-pattern-and-export-them.cs](./iterate-through-all-layers-select-those-matching-a-naming-pattern-and-export-them.cs) | iterate through all layers select those matching a naming pattern and export them |
| [perform-a-batch-export-by-iterating-over-layer-groups-assigning-to-cadbaseentity-array-and-exporting-each.cs](./perform-a-batch-export-by-iterating-over-layer-groups-assigning-to-cadbaseentity-array-and-exporting-each.cs) | perform a batch export by iterating over layer groups assigning to cadbaseentity array and exporting each |
| [populate-the-cadbaseentity-array-with-the-chosen-layer-objects-for-export.cs](./populate-the-cadbaseentity-array-with-the-chosen-layer-objects-for-export.cs) | populate the cadbaseentity array with the chosen layer objects for export |
| [retrieve-layer-names-filter-by-visibility-add-to-cadbaseentity-array-and-export.cs](./retrieve-layer-names-filter-by-visibility-add-to-cadbaseentity-array-and-export.cs) | retrieve layer names filter by visibility add to cadbaseentity array and export |
| [retrieve-the-list-of-all-layers-in-the-drawing-using-get-list-of-all-layers.cs](./retrieve-the-list-of-all-layers-in-the-drawing-using-get-list-of-all-layers.cs) | retrieve the list of all layers in the drawing using get list of all layers |
| [use-get-layout-names-to-list-available-layouts-before-selecting-a-specific-layout.cs](./use-get-layout-names-to-list-available-layouts-before-selecting-a-specific-layout.cs) | use get layout names to list available layouts before selecting a specific layout |
| [use-get-layout-names-to-select-a-layout-retrieve-its-layers-and-export-a-subset.cs](./use-get-layout-names-to-select-a-layout-retrieve-its-layers-and-export-a-subset.cs) | use get layout names to select a layout retrieve its layers and export a subset |
| [use-get-layout-names-to-select-the-target-layout-for-layer-export.cs](./use-get-layout-names-to-select-the-target-layout-for-layer-export.cs) | use get layout names to select the target layout for layer export |
| [use-get-list-of-all-layers-to-log-layer-names-before-and-after-selection-for-debugging.cs](./use-get-list-of-all-layers-to-log-layer-names-before-and-after-selection-for-debugging.cs) | use get list of all layers to log layer names before and after selection for debugging |
| [validate-that-the-exported-file-size-reflects-the-rescaling-performed-by-updatesize.cs](./validate-that-the-exported-file-size-reflects-the-rescaling-performed-by-updatesize.cs) | validate that the exported file size reflects the rescaling performed by updatesize |
| [verify-that-only-the-specified-layers-appear-in-the-exported-file-after-export.cs](./verify-that-only-the-specified-layers-appear-in-the-exported-file-after-export.cs) | verify that only the specified layers appear in the exported file after export |
| [verify-that-the-drawing-entities-collection-contains-only-the-cadbaseentity-array-after-assignment.cs](./verify-that-the-drawing-entities-collection-contains-only-the-cadbaseentity-array-after-assignment.cs) | verify that the drawing entities collection contains only the cadbaseentity array after assignment |

## Category Statistics
- Total examples: 29

## General Tips
- Follow root boundaries and testing guide.
- Dispose drawing images and streams; avoid hardcoded secrets; prefer explicit types.

<!-- AUTOGENERATED:START -->
| Date | Run ID | Branch/Commit |
|------|--------|---------------|
| 2026-10-29 | `cad-failed39-repairs-2026-10-29` | examples/verified-cad-repairs-2026-10-29 |
<!-- AUTOGENERATED:END -->
