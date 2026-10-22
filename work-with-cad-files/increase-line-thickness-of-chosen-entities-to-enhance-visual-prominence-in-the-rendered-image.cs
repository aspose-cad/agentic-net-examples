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
            string inputPath = "input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Configure rasterization options – increase LineScale to make lines thicker
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Millimeter,   // measurement unit
                PageWidth = 800,                  // width of the output image
                PageHeight = 600,                 // height of the output image
                LineScale = 5.0f                  // thicker lines (default is 1.0)
            };

            // Optional: set start and end caps for the rendered lines
            PenOptions penOptions = new PenOptions
            {
                StartCap = Aspose.CAD.LineCap.Round, // round start cap
                EndCap = Aspose.CAD.LineCap.Round    // round end cap
            };
            rasterOptions.PenOptions = penOptions;

            // Create PNG export options and attach the rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Path for the rendered image
            string outputPath = "output.png";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Render and save the image with increased line thickness
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}