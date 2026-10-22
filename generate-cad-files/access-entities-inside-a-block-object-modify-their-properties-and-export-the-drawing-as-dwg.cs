using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' not found.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                CadBlockEntity targetBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "MyBlock")
                    {
                        targetBlock = block;
                        break;
                    }
                }

                if (targetBlock != null)
                {
                    foreach (CadEntityBase entity in targetBlock.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            line.FirstPoint = new Cad3DPoint(line.FirstPoint.X + 10, line.FirstPoint.Y, line.FirstPoint.Z);
                            line.SecondPoint = new Cad3DPoint(line.SecondPoint.X + 10, line.SecondPoint.Y, line.SecondPoint.Z);
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            for (int i = 0; i < polyline.Coordinates.Count; i++)
                            {
                                var pt = polyline.Coordinates[i];
                                polyline.Coordinates[i] = new Cad2DPoint(pt.X * 2, pt.Y * 2);
                            }
                        }
                    }
                }

                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}