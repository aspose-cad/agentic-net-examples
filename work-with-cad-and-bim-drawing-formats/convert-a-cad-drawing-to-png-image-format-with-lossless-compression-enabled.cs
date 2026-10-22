using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        // Define input CAD file path (replace with an existing file)
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Source file not found: {inputPath}");
            return;
        }

        // Prepare output directory
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        // Define output PNG file path
        string outputPath = Path.Combine(outputDir, "output.png");

        try
        {
            // Load CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height
                };

                // Configure PNG save options with lossless compression
                var pngOptions = new PngOptions
                {
                    CompressionLevel = 0, // 0 = no compression (lossless)
                    VectorRasterizationOptions = rasterOptions
                };

                // Save as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Conversion successful. PNG saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}