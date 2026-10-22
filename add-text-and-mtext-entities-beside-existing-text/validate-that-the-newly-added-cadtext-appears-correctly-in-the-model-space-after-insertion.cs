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
                // Path to the source CAD file
                string inputPath = "sample.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path to the output PNG file
                string outputPath = "output.png";

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific object model
                    CadImage cadImage = (CadImage)image;

                    // List all layers with their properties
                    Console.WriteLine("Layers:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        string status = "";
                        if (layer.ColorId < 0)
                            status += "Off ";
                        if ((layer.Flags & 1) != 0)
                            status += "Frozen ";
                        if ((layer.Flags & 0x10) != 0)
                            status += "Locked ";
                        Console.WriteLine($"- Name: {layer.Name}, ColorId: {layer.ColorId}, Status: {status.Trim()}");
                    }

                    // List all block definitions
                    Console.WriteLine("\nBlock Definitions:");
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"- Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                        // List entities inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            PrintEntityInfo(entity, "  ");
                        }
                    }

                    // List top‑level entities (outside of blocks)
                    Console.WriteLine("\nTop‑Level Entities:");
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        PrintEntityInfo(entity, "  ");
                    }

                    // Save the drawing as PNG using rasterization options
                    var rasterOptions = new CadRasterizationOptions
                    {
                        // Optional: specify layout or layers to render
                        // Layouts = new[] { "Model" },
                        // Layers = new[] { "0" }
                        PageWidth = 1600,
                        PageHeight = 1200,
                        DrawType = CadDrawTypeMode.UseObjectColor
                    };

                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, pngOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

    // Helper method to print basic information about an entity
    static void PrintEntityInfo(CadEntityBase entity, string indent)
    {
        switch (entity)
        {
            case CadArc arc:
                Console.WriteLine($"{indent}Arc: Center=({arc.CenterPoint.X}, {arc.CenterPoint.Y}, {arc.CenterPoint.Z}), StartAngle={arc.StartAngle}, EndAngle={arc.EndAngle}, Layer={arc.LayerName}");
                break;
            case CadCircle circle:
                Console.WriteLine($"{indent}Circle: Center=({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z}), Radius={circle.Radius}, Layer={circle.LayerName}");
                break;
            case CadLine line:
                Console.WriteLine($"{indent}Line: Start=({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}), End=({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z}), Layer={line.LayerName}");
                break;
            case CadLwPolyline polyline:
                Console.WriteLine($"{indent}LwPolyline: PointsCount={polyline.Coordinates.Count}, Layer={polyline.LayerName}");
                break;
            case Cad3DFace face:
                Console.WriteLine($"{indent}3DFace: Corners=({face.FirstCorner.X},{face.FirstCorner.Y},{face.FirstCorner.Z})-({face.SecondCorner.X},{face.SecondCorner.Y},{face.SecondCorner.Z})-({face.ThirdCorner.X},{face.ThirdCorner.Y},{face.ThirdCorner.Z})-({face.FourthCorner.X},{face.FourthCorner.Y},{face.FourthCorner.Z}), Layer={face.LayerName}");
                break;
            case CadInsertObject insert:
                Console.WriteLine($"{indent}Insert: BlockName={insert.Name}, Position=({insert.InsertionPoint.X}, {insert.InsertionPoint.Y}, {insert.InsertionPoint.Z}), Scale=({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ}), Layer={insert.LayerName}");
                break;
            default:
                Console.WriteLine($"{indent}Entity: Type={entity.GetType().Name}, Layer={entity.LayerName}");
                break;
        }
    }
}