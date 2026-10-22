using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadEntityInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string cadFilePath = "sample.dwg";

                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                using (Image image = Image.Load(cadFilePath))
                {
                    CadImage cadImage = (CadImage)image;

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        Console.WriteLine($"Entity Type: {entity.TypeName}");

                        if (entity is CadLine line)
                        {
                            Console.WriteLine($"  Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                              $"to ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            Console.WriteLine($"  Lightweight Polyline with {polyline.Coordinates.Count} points");
                            foreach (Cad2DPoint pt in polyline.Coordinates)
                            {
                                Console.WriteLine($"    Point ({pt.X}, {pt.Y})");
                            }
                        }
                        else if (entity is CadInsertObject insert)
                        {
                            Console.WriteLine($"  Insert of block \"{insert.Name}\" at scale ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                        }
                    }

                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Definition: {block.Name} contains {block.Entities.Count} entities");
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            Console.WriteLine($"  Block Entity Type: {blockEntity.TypeName}");
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