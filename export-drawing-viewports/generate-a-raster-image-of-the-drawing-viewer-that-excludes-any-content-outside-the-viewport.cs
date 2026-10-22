using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string sourcePath = "input.dwg";

            // Verify that the source file exists
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source CAD file not found: " + sourcePath);
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "drawing.png");

            // Load the CAD drawing
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Configure rasterization options to match the drawing's viewport
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Use the drawing's original size as the page size to exclude content outside the viewport
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height
                    // Default drawing mode (object colors) will be used
                };

                // Set PNG export options with the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rasterized image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine("Raster image saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}