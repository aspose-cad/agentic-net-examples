using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path for the exported PNG image
                string outputPath = "output.png";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Configure rasterization options to keep original scale (disable automatic scaling)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        AutomaticLayoutsScaling = false,
                        Layouts = new[] { "Model" }
                    };

                    // Set up PNG export options with the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as a PNG image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}