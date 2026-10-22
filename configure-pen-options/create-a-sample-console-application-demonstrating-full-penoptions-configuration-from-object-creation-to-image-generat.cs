using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace PenOptionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (DWG, DXF, etc.)
            string inputPath = @"Samples/input.dwg";

            // Output PNG file path
            string outputPath = @"Output/output.png";

            try
            {
                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // -------------------------------------------------
                // Configure PenOptions – only StartCap and EndCap are supported
                // -------------------------------------------------
                PenOptions penOptions = new PenOptions
                {
                    // Use round start cap and square end cap as an example
                    StartCap = LineCap.Round,
                    EndCap   = LineCap.Square
                };

                // -------------------------------------------------
                // Configure rasterization options
                // -------------------------------------------------
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth  = 1600,
                    PageHeight = 1200,
                    // UnitType.Centimeter is the correct spelling in the SDK
                    UnitType   = UnitType.Centimenter,
                    // LineScale influences the visual thickness of lines
                    LineScale  = 0.5f,
                    // Attach the PenOptions to the rasterizer
                    PenOptions = penOptions
                };

                // -------------------------------------------------
                // Configure PNG export options
                // -------------------------------------------------
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Save the rendered PNG using the configured options
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Image successfully saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}