using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output.png");

            using (Image cadImage = Image.Load(inputPath))
            {
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    ObserverPoint = new ObserverPoint(30f, 45f, 0f),
                    Zoom = 1.0f
                };

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Rasterized image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}