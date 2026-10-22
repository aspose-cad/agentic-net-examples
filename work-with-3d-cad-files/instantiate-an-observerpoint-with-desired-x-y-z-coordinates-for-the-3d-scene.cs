using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;

namespace CadObserverExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dxf";

                // Path for the rendered PNG output
                string outputPath = "output.png";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD image
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Create rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        UnitType = UnitType.Millimeter,
                        LineScale = 0.5f,
                        // Define observer point using rotation angles (X, Y, Z) in degrees
                        ObserverPoint = new ObserverPoint(30f, 45f, 0f)
                    };

                    // Create PNG options and assign the rasterization options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine("Rendering completed. Output saved to " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}