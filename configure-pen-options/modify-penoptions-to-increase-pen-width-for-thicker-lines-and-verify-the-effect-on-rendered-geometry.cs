using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Input CAD file path
        string inputPath = "sample.dwg";
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Prepare output directory and file path
        string outputDir = "output";
        Directory.CreateDirectory(outputDir);
        string outputPath = Path.Combine(outputDir, "output.png");

        try
        {
            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                Layouts = new[] { "Model" },
                // Increase line scale for thicker lines
                LineScale = 2.0f
            };

            // Set PNG export options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rendered image
            cadImage.Save(outputPath, pngOptions);

            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}