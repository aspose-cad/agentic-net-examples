using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPenOptionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (DWG, DXF, etc.)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output raster image path
                string outputPath = "output.png";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD drawing
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // NOTE: Aspose.CAD PenOptions does not support dash styles (dash‑dot, dash, etc.).
                // Only StartCap and EndCap are available, and PenOptions is not applicable to PNG output.
                // Therefore we proceed with default pen rendering.

                // Configure rasterization options (controls rendering)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Millimeter,
                    PageWidth = 800,   // Width in pixels (or units based on UnitType)
                    PageHeight = 600,  // Height in pixels
                    LineScale = 1.0f,  // Controls thickness of rendered lines
                    AutomaticLayoutsScaling = true
                };

                // Create PNG options and assign the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);

                // Clean up
                cadImage.Dispose();

                Console.WriteLine("Rendering completed. Output saved to: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}