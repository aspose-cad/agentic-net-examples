# Aspose.CAD for .NET Examples

Build-validated C# examples for Aspose.CAD for .NET, organized for developers, AI coding agents, and LLM-based development tools.

## About
Agent-generated C# examples for Aspose.CAD for .NET, compiled, executed, and validated by an agentic pipeline. See [AGENTS.md](./AGENTS.md) for coding-agent instructions and [llms.txt](./llms.txt) for a machine-readable repository map. Use [search-index.json](./search-index.json) for search, SEO, and agent discovery metadata, and [examples-index.md](./examples-index.md) for a task-oriented example index.

[products.aspose.com/cad/net/](https://products.aspose.com/cad/net/)

## Overview
This repository provides working code examples demonstrating Aspose.CAD for .NET capabilities. All examples are automatically generated, compiled, and validated using the Aspose.CAD Examples Generator.

## Repair and validation status

This branch contains **1,895 examples across 45 categories**, targeting .NET 8 and Aspose.CAD 26.9.0. The October 30 fix recovers DXF insertion into an existing DWG and verifies preservation after reload. Together with the previous batch, **35 of the original 39 failures are recovered; four watermark export cases remain unresolved and excluded**. See [VALIDATION.md](./VALIDATION.md) for evidence, counts, and limits.

## Repository Structure
Examples are organized by feature category:
- `access-entities-inside-block-objects/` - 34 example(s)
- `access-polyline-and-3dface-vertices/` - 29 example(s)
- `add-background-images-to-dwg-files/` - 33 example(s)
- `add-background-images-to-dxf-files/` - 30 example(s)
- `add-margins-to-cad-drawings/` - 27 example(s)
- `add-polylines/` - 34 example(s)
- `add-text-and-mtext-entities-beside-existing-text/` - 35 example(s)
- `adjust-cad-drawing-properties/` - 34 example(s)
- `apply-blind-watermarks/` - 9 example(s)
- `calculate-circle-area-and-circumference/` - 30 example(s)
- `change-emergency-fonts-and-style-fonts/` - 28 example(s)
- `change-polyline-width/` - 30 example(s)
- `change-the-observer-point/` - 31 example(s)
- `configure-pen-options/` - 33 example(s)
- `configure-quality-options-for-text-hatches-and-arcs/` - 29 example(s)
- `convert-dwg-and-dxf-drawings-to-pdf-and-raster-images/` - 30 example(s)
- `create-hatched-rectangles-from-polylines/` - 35 example(s)
- `delete-entities-inside-a-rectangle/` - 30 example(s)
- `edit-object-hyperlinks/` - 30 example(s)
- `exclude-hidden-and-named-layers-from-export/` - 30 example(s)
- `export-cad-drawings-to-pdf-a/` - 27 example(s)
- `export-cad-files/` - 35 example(s)
- `export-drawing-viewports/` - 30 example(s)
- `export-full-layout-content/` - 39 example(s)
- `export-part-of-a-cad-image/` - 30 example(s)
- `export-selected-entities/` - 35 example(s)
- `export-selected-layers-with-rescaling/` - 29 example(s)
- `generate-cad-files/` - 35 example(s)
- `get-cad-layer-list/` - 30 example(s)
- `get-cad-layout-names/` - 30 example(s)
- `get-cad-layout-size/` - 35 example(s)
- `get-external-file-references/` - 31 example(s)
- `insert-dxf-or-dwg-drawings-into-dwg-files/` - 30 example(s)
- `optimize-hatch-origin-for-better-performance/` - 29 example(s)
- `remove-borders-from-the-output-canvas/` - 30 example(s)
- `remove-cad-entities/` - 29 example(s)
- `search-attributes-inside-insert-entities/` - 29 example(s)
- `validate-and-replace-external-image-references/` - 40 example(s)
- `work-with-3d-cad-files/` - 32 example(s)
- `work-with-cad-and-bim-drawing-formats/` - 526 example(s)
- `work-with-cad-entities/` - 35 example(s)
- `work-with-cad-files/` - 34 example(s)
- `work-with-cad-fonts/` - 29 example(s)
- `work-with-cad-layouts/` - 30 example(s)
- `work-with-cad-text/` - 35 example(s)

Each category contains standalone `.cs` files that can be compiled and run independently.

## Frequently Asked Questions

### Does a passing smoke test prove an exported drawing is correct?

No. Most examples require a local input drawing and exit safely when it is missing. Compile-and-run success verifies SDK compatibility and that guard; supply a representative drawing to verify geometry, layout, ownership, and the exported artifact.

### How do I load DWG or DXF files with Aspose.CAD for .NET?

Use `Image.Load(path)` from Aspose.CAD and cast or inspect the loaded image only after guarding the input path. Generated examples should keep file IO inside try/catch blocks.

### How do I convert CAD drawings to PDF or raster images?

Load the drawing with Aspose.CAD, configure the matching image or CAD rasterization options, and save to the requested target format. Keep output directories local and created by the sample.

### Which package version do these examples target?

These examples target Aspose.CAD for .NET 26.9.0 and .NET 8. Keep the package version pinned when reproducing their compile-and-run checks.

### Can generated examples depend on external services?

No. CAD examples should be standalone console samples. If a task mentions storage, UI, or automation integrations, the generated code should keep those boundaries as guarded placeholders unless the repository explicitly adds that dependency.

### How should missing source CAD files be handled?

Generated samples should check whether input files exist and exit gracefully with a concise message. Passing examples must compile and run without unhandled exceptions.

### Can these examples be used by AI coding agents like Claude, Copilot, or Cursor?

Yes. The repository includes root and per-category `AGENTS.md`, `llms.txt`, `readiness.json`, `index.json`, and `search-index.json` files so AI coding agents can navigate examples, categories, namespaces, and validation metadata programmatically.

### Do I need an Aspose.CAD license to run these examples?

Aspose.CAD can run in evaluation mode with product limitations, but production usage requires a valid license. Apply a license before production workflows and see https://purchase.aspose.com/buy or the temporary license page for evaluation options.

## Getting Started

### Prerequisites
- .NET SDK (net8.0 or compatible version)
- Aspose.CAD for .NET NuGet package
- Valid Aspose license (for production use)

### Running Examples

Each example is a self-contained C# file. To run an example:
```bash
cd <CategoryFolder>
dotnet new console --framework net8.0 -o ExampleProject
cd ExampleProject
dotnet add package Aspose.CAD --version 26.9.0
# Copy the example .cs file as Program.cs
dotnet run
```

## Code Patterns

### Loading a CAD drawing
```csharp
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

using Image drawing = Image.Load("input.dxf");
Console.WriteLine($"Drawing size: {drawing.Width} x {drawing.Height}");
```

### Error Handling
```csharp
if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"Error: File not found – {inputPath}");
    return;
}

try
{
    // Operations
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
}
```

### Important Notes
- Examples are single-file console applications; do not add multi-file projects.
- Dispose clients/streams with `using` when applicable.
- Avoid hardcoding secrets or license keys.

## Generation Architecture

This repository is produced by the Aspose.CAD product-specific examples generator. It uses the shared Examples RAG/MCP platform for retrieval and skeleton code generation, then applies Aspose.CAD-specific rules, validation, compile/run checks, repair, and publishing.

- The shared Task Generator supplies versioned product tasks.
- The shared Examples RAG/MCP platform retrieves product API context and generates skeleton code.
- The Aspose.CAD generator owns product-specific rules and compile/runtime guardrails.

## Agentic .NET Ecosystem

Other Aspose products with agentic, build-validated example repositories:

| Product | Repository | Focus |
|---------|------------|-------|
| Aspose.Words for .NET | [aspose-words/agentic-net-examples](https://github.com/aspose-words/agentic-net-examples) | Word processing, DOCX, mail merge |
| Aspose.Cells for .NET | [aspose-cells/agentic-net-examples](https://github.com/aspose-cells/agentic-net-examples) | Spreadsheets, Excel, charts |
| Aspose.PDF for .NET | [aspose-pdf/agentic-net-examples](https://github.com/aspose-pdf/agentic-net-examples) | PDF creation, conversion, document automation |
| Aspose.HTML for .NET | [aspose-html/agentic-net-examples](https://github.com/aspose-html/agentic-net-examples) | HTML conversion, DOM editing |
| Aspose.Imaging for .NET | [aspose-imaging/agentic-net-examples](https://github.com/aspose-imaging/agentic-net-examples) | Image conversion, manipulation |
| Aspose.Slides for .NET | [aspose-slides/agentic-net-examples](https://github.com/aspose-slides/agentic-net-examples) | Presentations, PowerPoint |
| Aspose.CAD for .NET | [aspose-cad/agentic-net-examples](https://github.com/aspose-cad/agentic-net-examples) | CAD drawings, conversion, rendering |
| Aspose.BarCode for .NET | [aspose-barcode/agentic-net-examples](https://github.com/aspose-barcode/agentic-net-examples) | Barcode generation and recognition |

## Related Resources

### Official Documentation
- [Aspose.CAD for .NET Documentation](https://docs.aspose.com/cad/net/) - Guides, tutorials, and feature overviews
- [API Reference](https://reference.aspose.com/cad/net/) - Complete class/method reference
- [Release Notes](https://releases.aspose.com/cad/net/release-notes/) - Version history and changelogs

### Downloads & Packages
- [NuGet Package](https://www.nuget.org/packages/Aspose.CAD) - Install via `dotnet add package Aspose.CAD`
- [Direct Downloads](https://releases.aspose.com/cad/net/) - MSI/ZIP installers and DLLs

### Community & Support
- [Aspose.CAD Forum](https://forum.aspose.com/c/cad/13) - Community Q&A and official support
- [Aspose Blog - CAD](https://blog.aspose.com/category/cad/) - Tutorials, tips, and product updates
- [GitHub Issues](https://github.com/aspose-cad/agentic-net-examples/issues) - Bug reports and feature requests

### AI-Friendly Navigation
- [Coding Agent Guide](./AGENTS.md) - Instructions for AI coding agents and code-generation tools
- [LLM Repository Map](./llms.txt) - Compact machine-readable navigation
- [Examples Index](./examples-index.md) - Task-oriented index for developers, search, and AI tools
- [Search Discovery Index](./search-index.json) - Machine-readable docs, examples, keywords, and API areas

### Licensing & Purchase
- [Purchase](https://purchase.aspose.com/buy) - Commercial license options
- [Temporary License](https://purchase.aspose.com/temporary-license/) - Full-feature evaluation license

## License
All examples use [Aspose.CAD for .NET](https://products.aspose.com/cad/net/) and require a valid license for production use. See [licensing options](https://purchase.aspose.com/buy).

---
*Generated and validated by a product-specific examples generator using shared Examples RAG/MCP infrastructure. See the [agentic examples metrics section](https://metrics.aspose.com/agents/sections/examples) | For AI-friendly guidance, see [AGENTS.md](./AGENTS.md) | Last updated: 2026-10-30*
