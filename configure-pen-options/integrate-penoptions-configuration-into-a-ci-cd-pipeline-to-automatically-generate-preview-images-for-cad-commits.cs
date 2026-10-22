using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input CAD file path (first argument or default placeholder)
            string inputPath = args.Length > 0 ? args[0] : "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input CAD file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + "_preview.png");

            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options for preview generation
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 800,   // preview width in pixels
                PageHeight = 600   // preview height in pixels
                // PenOptions configuration can be added here if supported by the library version
            };

            // Set up PNG save options with the rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the preview image
            cadImage.Save(outputPath, pngOptions);

            Console.WriteLine($"Preview image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}