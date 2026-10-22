using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input CAD file path (adjust as needed)
        string inputPath = "sample.dxf";

        // Verify source file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Source CAD file not found: {inputPath}");
            return;
        }

        // Prepare output directory
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
        try
        {
            Directory.CreateDirectory(outputDir);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        try
        {
            // Load CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Enumerate layers and display visibility information
                Console.WriteLine("Layers:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    bool isLocked = (layer.Flags & 16) != 0; // bit 4
                    Console.WriteLine($"  Name: {layer.Name}, ColorId: {layer.ColorId}, Off: {isOff}, Frozen: {isFrozen}, Locked: {isLocked}");
                }

                // Enumerate top‑level entities
                Console.WriteLine("\nTop‑level Entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    PrintEntityInfo(entity, "  ");
                }

                // Enumerate block definitions
                Console.WriteLine("\nBlock Definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"  Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        PrintEntityInfo(blockEntity, "    ");
                    }
                }

                // Render CAD to PDF
                string pdfPath = Path.Combine(outputDir, "Rendered.pdf");
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new string[] { "Model" }
                };
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"\nPDF rendering saved to: {pdfPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }

    // Helper method to print entity details based on type
    static void PrintEntityInfo(CadEntityBase entity, string indent)
    {
        // Common properties
        string layerInfo = $"Layer: {entity.LayerName}, ColorId: {entity.ColorId}";

        switch (entity)
        {
            case CadLine line:
                Console.WriteLine($"{indent}Line - {layerInfo}, Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}), End: ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                break;

            case CadArc arc:
                Console.WriteLine($"{indent}Arc - {layerInfo}, Center: ({arc.CenterPoint.X}, {arc.CenterPoint.Y}, {arc.CenterPoint.Z}), StartAngle: {arc.StartAngle}, EndAngle: {arc.EndAngle}");
                break;

            case CadCircle circle:
                Console.WriteLine($"{indent}Circle - {layerInfo}, Center: ({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z})");
                break;

            case CadLwPolyline polyline:
                Console.Write($"{indent}LwPolyline - {layerInfo}, Points: ");
                foreach (Cad2DPoint pt in polyline.Coordinates)
                {
                    Console.Write($"({pt.X},{pt.Y}) ");
                }
                Console.WriteLine();
                break;

            case Cad3DFace face:
                Console.WriteLine($"{indent}3DFace - {layerInfo}, Corners: ({face.FirstCorner.X},{face.FirstCorner.Y},{face.FirstCorner.Z}) ({face.SecondCorner.X},{face.SecondCorner.Y},{face.SecondCorner.Z}) ({face.ThirdCorner.X},{face.ThirdCorner.Y},{face.ThirdCorner.Z}) ({face.FourthCorner.X},{face.FourthCorner.Y},{face.FourthCorner.Z})");
                break;

            case CadInsertObject insert:
                Console.WriteLine($"{indent}Insert - {layerInfo}, Block: {insert.Name ?? insert.OriginalBlockName}, InsertionPoint: ({insert.InsertionPoint.X},{insert.InsertionPoint.Y},{insert.InsertionPoint.Z}), Scale: ({insert.ScaleX},{insert.ScaleY},{insert.ScaleZ}), Rotation: {insert.RotationAngle}");
                break;

            default:
                Console.WriteLine($"{indent}Entity of type {entity.GetType().Name} - {layerInfo}");
                break;
        }
    }
}