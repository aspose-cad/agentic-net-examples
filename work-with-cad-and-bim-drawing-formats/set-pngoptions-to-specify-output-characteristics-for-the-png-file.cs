using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Input CAD file path
        string inputPath = "input.dwg";

        // Output directory and PNG file path
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.png");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options for PNG rendering
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,               // Desired image width in pixels
                    PageHeight = 1200,              // Desired image height in pixels
                    AutomaticLayoutsScaling = true,// Scale layouts automatically
                    BorderX = 10,                   // Horizontal border (pixels)
                    BorderY = 10                    // Vertical border (pixels)
                    // Layouts can be set if specific layout names are required
                };

                // Set PNG export options and attach rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                    // BackgroundColor is optional; omitted to avoid System.Drawing dependency
                };

                // Save the CAD drawing as a PNG file
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"PNG file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}