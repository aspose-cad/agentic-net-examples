using System;
using Aspose.CAD;
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
                // Path to the CAD file
                string inputPath = "sample.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate through top-level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Get entity type name
                        string typeName = entity.GetType().Name;

                        // Get layer name
                        string layerName = entity.LayerName;

                        // Example attribute match: check if entity is on layer "0"
                        if (layerName == "0")
                        {
                            Console.WriteLine($"Entity Type: {typeName}, Layer: {layerName} (matched)");
                        }
                        else
                        {
                            Console.WriteLine($"Entity Type: {typeName}, Layer: {layerName}");
                        }

                        // Additional handling based on specific entity types
                        if (entity is CadLine line)
                        {
                            // Access line endpoints
                            double x1 = line.FirstPoint.X;
                            double y1 = line.FirstPoint.Y;
                            double z1 = line.FirstPoint.Z;

                            double x2 = line.SecondPoint.X;
                            double y2 = line.SecondPoint.Y;
                            double z2 = line.SecondPoint.Z;

                            // Example: output line length
                            double dx = x2 - x1;
                            double dy = y2 - y1;
                            double dz = z2 - z1;
                            double length = Math.Sqrt(dx * dx + dy * dy + dz * dz);
                            Console.WriteLine($"  Line length: {length}");
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            // Iterate through polyline points
                            foreach (Cad2DPoint point in polyline.Coordinates)
                            {
                                Console.WriteLine($"  Polyline point: ({point.X}, {point.Y})");
                            }
                        }
                        else if (entity is CadInsertObject insert)
                        {
                            // Access inserted block name and scales
                            string blockName = insert.Name;
                            double scaleX = insert.ScaleX;
                            double scaleY = insert.ScaleY;
                            double scaleZ = insert.ScaleZ;
                            Console.WriteLine($"  Inserted block: {blockName}, Scales: ({scaleX}, {scaleY}, {scaleZ})");
                        }
                        // Add more entity type checks as needed
                    }

                    // Optionally, iterate through block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block definition: {block.Name}, contains {block.Entities.Count} entities.");
                        // Example: inspect entities inside the block
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            Console.WriteLine($"  Block entity type: {blockEntity.GetType().Name}");
                        }
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}