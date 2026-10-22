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
            // Path to the source CAD file
            string inputPath = "sample.dwg";

            // Verify that the source file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input CAD file not found: {inputPath}");
                return;
            }

            // Create output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "rendered.png");

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,   // Desired image width
                    PageHeight = 1200,  // Desired image height
                    LineScale = 1.0f    // Adjust line thickness if needed
                    // Note: Custom pen color, width, and dash style are not exposed in the current API.
                };

                // Set PNG save options and attach rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
            }

            // Simple verification that the image was created
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"Rendered image saved successfully to: {outputPath}");
                // Further verification (e.g., pixel color checks) can be added here if needed.
            }
            else
            {
                Console.WriteLine("Failed to save the rendered image.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}