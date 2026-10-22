using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

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
                // Configure rasterization options with NoScaling disabled automatic scaling
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.NoScaling = true;
                rasterOptions.PageWidth = 1600;
                rasterOptions.PageHeight = 1200;
                rasterOptions.Layouts = new string[] { "Model" };

                // Set PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}