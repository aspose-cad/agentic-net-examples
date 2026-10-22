using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "sample.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "sample.png");

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the CAD drawing
            using (var cadImage = (Aspose.CAD.Image)Image.Load(inputPath))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    // Use the original image dimensions for the output PNG
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height,
                    // Additional properties can be set here, e.g., Layouts, DrawColorMode, etc.
                };

                // Configure PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions,
                    // CompressionLevel = 9; // optional compression setting
                };

                // Save the drawing as PNG
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Conversion completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}