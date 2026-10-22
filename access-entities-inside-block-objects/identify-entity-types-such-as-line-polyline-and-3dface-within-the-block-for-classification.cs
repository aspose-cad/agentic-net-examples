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
        const string filePath = "sample.dwg";
        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");

                    foreach (CadEntityBase entity in block.Entities)
                    {
                        switch (entity)
                        {
                            case CadLine line:
                                Console.WriteLine(
                                    $"  Line: Start({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                    $"End({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                                break;

                            case CadLwPolyline polyline:
                                Console.WriteLine("  Lightweight Polyline points:");
                                foreach (Cad2DPoint pt in polyline.Coordinates)
                                {
                                    Console.WriteLine($"    ({pt.X}, {pt.Y})");
                                }
                                break;

                            case Cad3DFace face:
                                Console.WriteLine("  3DFace corners:");
                                Console.WriteLine($"    1: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z})");
                                Console.WriteLine($"    2: ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z})");
                                Console.WriteLine($"    3: ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z})");
                                Console.WriteLine($"    4: ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                                break;

                            case CadInsertObject insert:
                                Console.WriteLine($"  Inserted Block Reference: {insert.Name}");
                                break;

                            default:
                                Console.WriteLine($"  Other entity type: {entity.GetType().Name}");
                                break;
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