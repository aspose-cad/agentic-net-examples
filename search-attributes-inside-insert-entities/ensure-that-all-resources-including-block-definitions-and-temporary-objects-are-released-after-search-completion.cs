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
        const string cadFilePath = "sample.dwg";

        if (!File.Exists(cadFilePath))
        {
            Console.Error.WriteLine($"File not found: {cadFilePath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = (CadImage)image;

                // Iterate through block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");

                    // Iterate through entities inside the block
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

                // Optionally process top-level drawing entities
                foreach (CadEntityBase topEntity in cadImage.Entities)
                {
                    Console.WriteLine($"Top-level entity type: {topEntity.GetType().Name}");
                }
            } // Image disposed, releasing all resources
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}