using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source DWG file
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path to the output PDF file
                string outputPath = "output.pdf";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific object model
                    CadImage cadImage = (CadImage)image;

                    // Enumerate layers and display basic information
                    Console.WriteLine("Layers:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isFrozen = (layer.Flags & 1) != 0;
                        bool isOff = layer.ColorId < 0;
                        bool isLocked = (layer.Flags & 0x10) != 0; // bit 4 indicates locked
                        Console.WriteLine($"- {layer.Name} (Frozen: {isFrozen}, Off: {isOff}, Locked: {isLocked})");
                    }

                    // Enumerate top‑level entities
                    Console.WriteLine("\nTop‑level entities:");
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        PrintEntityInfo(entity);
                    }

                    // Enumerate block definitions and their entities
                    Console.WriteLine("\nBlock definitions:");
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block: {block.Name}");
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            PrintEntityInfo(blockEntity, "  ");
                        }
                    }

                    // Prepare PDF export options with rasterization settings
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            // Export all layers; you can filter by name if needed
                            // Layers = new string[] { "Layer1", "Layer2" }
                        }
                    };

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

    // Helper method to print basic information about a CAD entity
    static void PrintEntityInfo(CadEntityBase entity, string indent = "")
    {
        switch (entity)
        {
            case CadLine line:
                Console.WriteLine($"{indent}Line: Start({line.FirstPoint.X}, {line.FirstPoint.Y}), End({line.SecondPoint.X}, {line.SecondPoint.Y}), Layer={line.LayerName}");
                break;
            case CadArc arc:
                Console.WriteLine($"{indent}Arc: Center({arc.CenterPoint.X}, {arc.CenterPoint.Y}), Radius={arc.Radius}, StartAngle={arc.StartAngle}, EndAngle={arc.EndAngle}, Layer={arc.LayerName}");
                break;
            case CadCircle circle:
                Console.WriteLine($"{indent}Circle: Center({circle.CenterPoint.X}, {circle.CenterPoint.Y}), Radius={circle.Radius}, Layer={circle.LayerName}");
                break;
            case CadLwPolyline polyline:
                Console.WriteLine($"{indent}LwPolyline: Points={polyline.Coordinates.Count}, Layer={polyline.LayerName}");
                break;
            case CadInsertObject insert:
                Console.WriteLine($"{indent}Insert: BlockName={insert.Name}, InsertionPoint({insert.InsertionPoint.X}, {insert.InsertionPoint.Y}), Scale({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ}), Rotation={insert.RotationAngle}, Layer={insert.LayerName}");
                break;
            case Cad3DFace face:
                Console.WriteLine($"{indent}3DFace: Corners=({face.FirstCorner.X},{face.FirstCorner.Y},{face.FirstCorner.Z}) ... ({face.FourthCorner.X},{face.FourthCorner.Y},{face.FourthCorner.Z}), Layer={face.LayerName}");
                break;
            default:
                Console.WriteLine($"{indent}Entity type: {entity.GetType().Name}, Layer={entity.LayerName}");
                break;
        }
    }
}