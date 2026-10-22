using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // List layers
                Console.WriteLine("Layers:");
                var visibleLayerNames = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    string name = layer.Name;
                    bool frozen = (layer.Flags & 1) != 0;
                    bool locked = (layer.Flags & 0x10) != 0;
                    bool off = layer.ColorId < 0;
                    Console.WriteLine($"- {name} (Frozen={frozen}, Locked={locked}, Off={off})");
                    if (!off)
                        visibleLayerNames.Add(name);
                }

                // List top‑level entities
                Console.WriteLine("\nTop‑level entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    PrintEntityInfo(entity);
                }

                // List block definitions and their entities
                Console.WriteLine("\nBlock definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}, Entities count: {block.Entities.Count}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        PrintEntityInfo(blockEntity, indent: "  ");
                    }
                }

                if (visibleLayerNames.Count == 0)
                {
                    Console.WriteLine("No visible layers to export. Skipping PDF generation.");
                    return;
                }

                // Ensure output directory exists
                string outDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outDir))
                    Directory.CreateDirectory(outDir);

                // Export to PDF with only visible layers
                var rasterOptions = new CadRasterizationOptions
                {
                    Layers = visibleLayerNames.ToArray(),
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"\nExported PDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void PrintEntityInfo(CadEntityBase entity, string indent = "")
    {
        switch (entity)
        {
            case CadArc arc:
                Console.WriteLine($"{indent}Arc: Center=({arc.CenterPoint.X}, {arc.CenterPoint.Y}), StartAngle={arc.StartAngle}, EndAngle={arc.EndAngle}");
                break;
            case CadCircle circle:
                Console.WriteLine($"{indent}Circle: Center=({circle.CenterPoint.X}, {circle.CenterPoint.Y}), Radius={circle.Radius}");
                break;
            case CadLine line:
                Console.WriteLine($"{indent}Line: ({line.FirstPoint.X}, {line.FirstPoint.Y}) -> ({line.SecondPoint.X}, {line.SecondPoint.Y})");
                break;
            case CadLwPolyline polyline:
                Console.WriteLine($"{indent}LwPolyline: {polyline.Coordinates.Count} vertices");
                break;
            case Cad3DFace face:
                Console.WriteLine($"{indent}3DFace: 4 corners");
                break;
            case CadInsertObject insert:
                Console.WriteLine($"{indent}Insert: Name={insert.Name}, ScaleX={insert.ScaleX}, ScaleY={insert.ScaleY}, ScaleZ={insert.ScaleZ}");
                break;
            default:
                Console.WriteLine($"{indent}Entity type: {entity.GetType().Name}");
                break;
        }
    }
}