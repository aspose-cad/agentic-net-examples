using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            // Load a DWG/DXF file
            using (Image image = Image.Load(filePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Iterate over block definitions in the drawing
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Output basic block information
                    Console.WriteLine($"Block Base Point: ({block.BasePoint.X}, {block.BasePoint.Y}, {block.BasePoint.Z})");

                    // Access internal entities of the block
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        // Log the entity type
                        Console.WriteLine($"  Entity Type: {entity.GetType().Name}");

                        // Handle specific entity types
                        if (entity is CadLine line)
                        {
                            Console.WriteLine($"    Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                              $"to ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            Console.WriteLine("    Polyline points:");
                            foreach (var pt in polyline.Coordinates)
                            {
                                Console.WriteLine($"      ({pt.X}, {pt.Y})");
                            }
                        }
                        else if (entity is Cad3DFace face)
                        {
                            Console.WriteLine($"    3DFace corners: " +
                                              $"({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), " +
                                              $"({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), " +
                                              $"({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), " +
                                              $"({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                        }
                        else if (entity is CadInsertObject insert)
                        {
                            Console.WriteLine($"    Insert of block '{insert.Name}' at insertion point ({insert.InsertionPoint.X}, {insert.InsertionPoint.Y}, {insert.InsertionPoint.Z})");
                            Console.WriteLine($"    Scale: ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}