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
            // Input DWG file path (replace with actual path)
            string inputPath = "input.dwg";

            // Output PNG file path
            string outputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Define the region to export (example values)
            float regionCenterX = 100.0f;   // X coordinate of the region center
            float regionCenterY = 200.0f;   // Y coordinate of the region center
            int regionWidth = 500;          // Width of the exported region in pixels
            int regionHeight = 400;         // Height of the exported region in pixels

            // Load the DWG file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options to focus on the specified region
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = regionWidth,
                    PageHeight = regionHeight,
                    // Approximate centering by adjusting borders
                    BorderX = -regionCenterX + regionWidth / 2.0f,
                    BorderY = -regionCenterY + regionHeight / 2.0f
                };

                // Set PNG export options with the rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the specified region as a PNG file
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Region exported successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}