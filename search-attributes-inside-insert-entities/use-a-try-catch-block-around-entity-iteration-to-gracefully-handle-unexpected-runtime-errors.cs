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
            Console.WriteLine($"File '{filePath}' not found.");
            return;
        }

        try
        {
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}, Entities count: {block.Entities.Count}");

                    try
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            switch (entity)
                            {
                                case CadLine line:
                                    Console.WriteLine($"  Line: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) -> ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                                    break;

                                case CadLwPolyline polyline:
                                    Console.WriteLine("  LwPolyline points:");
                                    foreach (Cad2DPoint pt in polyline.Coordinates)
                                    {
                                        Console.WriteLine($"    ({pt.X}, {pt.Y})");
                                    }
                                    break;

                                case Cad3DFace face:
                                    Console.WriteLine($"  3DFace corners: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                                    break;

                                case CadInsertObject insert:
                                    Console.WriteLine($"  Insert: {insert.Name}, ScaleX={insert.ScaleX}, ScaleY={insert.ScaleY}, ScaleZ={insert.ScaleZ}");
                                    break;

                                default:
                                    Console.WriteLine($"  Other entity type: {entity.GetType().Name}");
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  Error processing block '{block.Name}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
        }
    }
}