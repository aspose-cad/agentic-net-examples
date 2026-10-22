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
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Aspose.CAD.Image image = Aspose.CAD.Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    ProcessEntity(entity);
                }

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        ProcessEntity(blockEntity);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void ProcessEntity(CadEntityBase entity)
    {
        switch (entity)
        {
            case CadLine line:
                Cad3DPoint start = line.FirstPoint;
                Cad3DPoint end = line.SecondPoint;
                Console.WriteLine($"Line: Start({start.X}, {start.Y}, {start.Z}) End({end.X}, {end.Y}, {end.Z})");
                break;

            case CadLwPolyline polyline:
                Console.WriteLine("Lightweight Polyline:");
                foreach (Cad2DPoint pt in polyline.Coordinates)
                {
                    Console.WriteLine($"  Vertex({pt.X}, {pt.Y})");
                }
                break;

            case Cad3DFace face:
                Cad3DPoint c1 = face.FirstCorner;
                Cad3DPoint c2 = face.SecondCorner;
                Cad3DPoint c3 = face.ThirdCorner;
                Cad3DPoint c4 = face.FourthCorner;
                Console.WriteLine("3D Face:");
                Console.WriteLine($"  Corner1({c1.X}, {c1.Y}, {c1.Z})");
                Console.WriteLine($"  Corner2({c2.X}, {c2.Y}, {c2.Z})");
                Console.WriteLine($"  Corner3({c3.X}, {c3.Y}, {c3.Z})");
                Console.WriteLine($"  Corner4({c4.X}, {c4.Y}, {c4.Z})");
                break;

            case CadInsertObject insert:
                string blockName = insert.Name ?? insert.OriginalBlockName;
                Console.WriteLine($"Insert: BlockName={blockName} ScaleX={insert.ScaleX} ScaleY={insert.ScaleY} ScaleZ={insert.ScaleZ}");
                break;

            default:
                Console.WriteLine($"Entity type: {entity.GetType().Name}");
                break;
        }
    }
}