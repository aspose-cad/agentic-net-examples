using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file
            string filePath = "sample.dwg";

            // Verify that the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            // Load the CAD image
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                // Name of the block to locate
                string targetBlockName = "MyBlock";

                // Locate the block definition
                CadBlockEntity targetBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == targetBlockName)
                    {
                        targetBlock = block;
                        break;
                    }
                }

                if (targetBlock == null)
                {
                    Console.WriteLine($"Block '{targetBlockName}' not found.");
                    return;
                }

                Console.WriteLine($"Block '{targetBlock.Name}' contains {targetBlock.Entities.Count} entities.");

                // Analyze the contents of the block
                foreach (CadEntityBase entity in targetBlock.Entities)
                {
                    string typeName = entity.GetType().Name;
                    Console.Write($"- {typeName}");

                    if (entity is CadLine line)
                    {
                        Console.WriteLine($": Start({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                          $"End({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                    }
                    else if (entity is CadLwPolyline polyline)
                    {
                        Console.WriteLine($": {polyline.Coordinates.Count} points");
                    }
                    else if (entity is Cad3DFace face)
                    {
                        Console.WriteLine($": Corners ({face.FirstCorner.X},{face.FirstCorner.Y},{face.FirstCorner.Z}), " +
                                          $"({face.SecondCorner.X},{face.SecondCorner.Y},{face.SecondCorner.Z}), " +
                                          $"({face.ThirdCorner.X},{face.ThirdCorner.Y},{face.ThirdCorner.Z}), " +
                                          $"({face.FourthCorner.X},{face.FourthCorner.Y},{face.FourthCorner.Z})");
                    }
                    else if (entity is CadInsertObject insert)
                    {
                        Console.WriteLine($": Insert of block '{insert.Name}' at scale ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                    }
                    else
                    {
                        Console.WriteLine();
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