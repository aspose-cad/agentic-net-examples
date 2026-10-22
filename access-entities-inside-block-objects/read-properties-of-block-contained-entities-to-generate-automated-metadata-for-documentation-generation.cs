using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadBlockMetadata
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the CAD file (DWG/DXF)
                    string inputPath = "sample.dwg";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD file and cast to CadImage to access CAD object model
                    using (Image image = Image.Load(inputPath))
                    {
                        CadImage cadImage = (CadImage)image;

                        // Iterate through each block definition in the drawing
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            // Output block name
                            Console.WriteLine($"Block: {block.Name}");

                            // Iterate through entities contained in the block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                // Common metadata: type and layer name
                                string typeName = entity.GetType().Name;
                                string layerName = entity.LayerName;
                                Console.Write($"  Entity: {typeName}, Layer: {layerName}");

                                // Detailed metadata based on entity type
                                if (entity is CadLine line)
                                {
                                    Console.WriteLine($", Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}), End: ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                                }
                                else if (entity is CadLwPolyline polyline)
                                {
                                    Console.Write(", Points: ");
                                    foreach (Cad2DPoint point in polyline.Coordinates)
                                    {
                                        Console.Write($"({point.X}, {point.Y}) ");
                                    }
                                    Console.WriteLine();
                                }
                                else if (entity is Cad3DFace face)
                                {
                                    Console.WriteLine($", Corners: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                                }
                                else if (entity is CadInsertObject insert)
                                {
                                    Console.WriteLine($", Inserted Block: {insert.Name}, Scale: ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                                }
                                else
                                {
                                    // For other entity types, just end the line
                                    Console.WriteLine();
                                }
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
}