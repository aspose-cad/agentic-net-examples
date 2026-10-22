using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

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

                // Load the CAD file and cast to CadImage
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Iterate over block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Name: {block.Name}");

                        // Iterate over entities inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // Log the entity type
                            string typeName = entity.TypeName.ToString();
                            Console.WriteLine($"  Entity Type: {typeName}");

                            // Detailed logging based on entity type
                            if (entity is CadLine line)
                            {
                                Console.WriteLine($"    First Point: X={line.FirstPoint.X}, Y={line.FirstPoint.Y}, Z={line.FirstPoint.Z}");
                                Console.WriteLine($"    Second Point: X={line.SecondPoint.X}, Y={line.SecondPoint.Y}, Z={line.SecondPoint.Z}");
                            }
                            else if (entity is CadLwPolyline polyline)
                            {
                                int index = 0;
                                foreach (Cad2DPoint pt in polyline.Coordinates)
                                {
                                    Console.WriteLine($"    Vertex {index}: X={pt.X}, Y={pt.Y}");
                                    index++;
                                }
                            }
                            else if (entity is Cad3DFace face)
                            {
                                Console.WriteLine($"    First Corner: X={face.FirstCorner.X}, Y={face.FirstCorner.Y}, Z={face.FirstCorner.Z}");
                                Console.WriteLine($"    Second Corner: X={face.SecondCorner.X}, Y={face.SecondCorner.Y}, Z={face.SecondCorner.Z}");
                                Console.WriteLine($"    Third Corner: X={face.ThirdCorner.X}, Y={face.ThirdCorner.Y}, Z={face.ThirdCorner.Z}");
                                Console.WriteLine($"    Fourth Corner: X={face.FourthCorner.X}, Y={face.FourthCorner.Y}, Z={face.FourthCorner.Z}");
                            }
                            else if (entity is CadInsertObject insert)
                            {
                                Console.WriteLine($"    Inserted Block Name: {insert.Name}");
                                Console.WriteLine($"    ScaleX: {insert.ScaleX}, ScaleY: {insert.ScaleY}, ScaleZ: {insert.ScaleZ}");
                            }
                            else
                            {
                                // For other entity types, just output that no detailed handling is implemented
                                Console.WriteLine("    No detailed logging implemented for this entity type.");
                            }
                        }

                        Console.WriteLine(); // Blank line between blocks
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}