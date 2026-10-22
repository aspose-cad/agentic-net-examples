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
            // Prepare input and output directories
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);

            // Path to the source CAD file
            string inputPath = Path.Combine(dataDir, "sample.dwg");
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Input CAD file not found: " + inputPath);
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,   // desired width in pixels
                    PageHeight = 1200   // desired height in pixels
                    // Note: Viewport selection is not available in the current API version.
                };

                // Set PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the raster snapshot
                string outputPath = Path.Combine(outputDir, "snapshot.png");
                cad.Save(outputPath, pngOptions);
                Console.WriteLine("Raster snapshot saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}