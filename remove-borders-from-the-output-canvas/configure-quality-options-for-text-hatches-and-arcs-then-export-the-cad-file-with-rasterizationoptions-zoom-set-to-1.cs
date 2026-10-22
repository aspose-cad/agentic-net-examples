using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories
                string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);

                // Path to the source CAD file
                string sourcePath = Path.Combine(dataDir, "sample.dwg");

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("Source CAD file not found: " + sourcePath);
                    return;
                }

                // Load the CAD image
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Configure rasterization options with zoom and page size
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Zoom = 1,                     // Set zoom factor
                        PageWidth = 1600,             // Optional page width
                        PageHeight = 1200             // Optional page height
                    };

                    // Set up PNG export options and attach rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Define output file path
                    string outputPath = Path.Combine(outputDir, "exported_image.png");

                    // Save the rasterized image
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine("Export completed: " + outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}