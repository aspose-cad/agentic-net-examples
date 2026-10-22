using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the input CAD file (DWG/DXF)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Path to the debug log file
            string logPath = "block_entities_log.txt";

            // Load the CAD image
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model members
                CadImage cadImage = (CadImage)image;

                using (StreamWriter writer = new StreamWriter(logPath, false))
                {
                    // Iterate over block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        writer.WriteLine($"Block: {block.Name}");

                        // Iterate over entities inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            string typeName = entity.TypeName.ToString();
                            writer.Write($"  Entity Type: {typeName}");

                            // Log attributes based on the concrete entity type
                            switch (entity)
                            {
                                case CadLine line:
                                    writer.WriteLine($", Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}), End: ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                                    break;
                                case CadLwPolyline polyline:
                                    writer.Write(", Points: ");
                                    foreach (var pt in polyline.Coordinates)
                                    {
                                        writer.Write($"({pt.X}, {pt.Y}) ");
                                    }
                                    writer.WriteLine();
                                    break;
                                case Cad3DFace face:
                                    writer.WriteLine($", Corners: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                                    break;
                                case CadInsertObject insert:
                                    writer.WriteLine($", Inserted Block: {insert.Name}, Scale: ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                                    break;
                                default:
                                    writer.WriteLine();
                                    break;
                            }
                        }

                        writer.WriteLine();
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