using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadLineThicknessExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.dxf";
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine("Input CAD file not found.");
                    return;
                }

                // Load the CAD drawing
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Configure rasterization options to increase line thickness.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Millimeter,   // Use millimeters for scaling (optional)
                    LineScale = 2.0f,                 // Increase line thickness by a factor of 2
                    PageWidth = 2000f,                // Desired output width in pixels (optional)
                    PageHeight = 2000f,               // Desired output height in pixels (optional)
                    AutomaticLayoutsScaling = true   // Scale layouts automatically (optional)
                };

                // Create PNG export options and assign the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Ensure output directory exists
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "output.png");

                // Save the rendered image with increased line thickness
                cadImage.Save(outputPath, pngOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}