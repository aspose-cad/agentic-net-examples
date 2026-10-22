using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadBlockInspection
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the CAD file (DWG/DXF)
                    string inputPath = "input.dwg";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD file
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Iterate over block definitions
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            Console.WriteLine($"Block: {block.Name}");

                            // Iterate over entities inside the block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                string typeName = entity.GetType().Name;
                                string layerName = entity.LayerName;
                                Console.Write($"  Entity Type: {typeName}, Layer: {layerName}, ");

                                // Handle specific entity types
                                if (entity is CadLine line)
                                {
                                    Cad3DPoint first = line.FirstPoint;
                                    Cad3DPoint second = line.SecondPoint;
                                    Console.WriteLine($"Line from ({first.X}, {first.Y}, {first.Z}) to ({second.X}, {second.Y}, {second.Z})");
                                }
                                else if (entity is CadLwPolyline polyline)
                                {
                                    Console.Write("Polyline points: ");
                                    foreach (Cad2DPoint pt in polyline.Coordinates)
                                    {
                                        Console.Write($"({pt.X}, {pt.Y}) ");
                                    }
                                    Console.WriteLine();
                                }
                                else if (entity is Cad3DFace face)
                                {
                                    Cad3DPoint c1 = face.FirstCorner;
                                    Cad3DPoint c2 = face.SecondCorner;
                                    Cad3DPoint c3 = face.ThirdCorner;
                                    Cad3DPoint c4 = face.FourthCorner;
                                    Console.WriteLine($"3DFace corners: ({c1.X},{c1.Y},{c1.Z}) ({c2.X},{c2.Y},{c2.Z}) ({c3.X},{c3.Y},{c3.Z}) ({c4.X},{c4.Y},{c4.Z})");
                                }
                                else if (entity is CadInsertObject insert)
                                {
                                    string originalBlockName = insert.OriginalBlockName;
                                    double scaleX = insert.ScaleX;
                                    double scaleY = insert.ScaleY;
                                    double scaleZ = insert.ScaleZ;
                                    Console.WriteLine($"Insert of block '{originalBlockName}' with scales ({scaleX}, {scaleY}, {scaleZ})");
                                }
                                else
                                {
                                    Console.WriteLine("Other entity");
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