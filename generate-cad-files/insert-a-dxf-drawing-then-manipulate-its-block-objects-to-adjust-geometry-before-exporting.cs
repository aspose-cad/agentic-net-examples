using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            line.FirstPoint = new Cad3DPoint(
                                line.FirstPoint.X + 10,
                                line.FirstPoint.Y,
                                line.FirstPoint.Z);

                            line.SecondPoint = new Cad3DPoint(
                                line.SecondPoint.X + 10,
                                line.SecondPoint.Y,
                                line.SecondPoint.Z);
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            for (int i = 0; i < polyline.Coordinates.Count; i++)
                            {
                                var pt = polyline.Coordinates[i];
                                polyline.Coordinates[i] = new Cad2DPoint(
                                    pt.X + 10,
                                    pt.Y);
                            }
                        }
                    }
                }

                var pngOptions = new PngOptions();
                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Saved modified drawing to '{outputPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"Error saving file: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}