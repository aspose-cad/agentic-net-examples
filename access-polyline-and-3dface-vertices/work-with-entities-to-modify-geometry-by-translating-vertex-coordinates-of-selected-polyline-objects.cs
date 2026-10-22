using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                double offsetX = 10.0;
                double offsetY = 5.0;

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLwPolyline polyline)
                    {
                        for (int i = 0; i < polyline.Coordinates.Count; i++)
                        {
                            Cad2DPoint pt = polyline.Coordinates[i];
                            pt.X += offsetX;
                            pt.Y += offsetY;
                            polyline.Coordinates[i] = pt;
                        }
                    }
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}