using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string sourcePath = "input.dwg";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Path for the rendered output image
        string outputPath = "output.png";

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD document
            CadImage cadImage = (CadImage)Image.Load(sourcePath);

            // Configure rasterization options for rendering
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Millimeter,
                PageWidth = 800,   // Width of the output image in pixels
                PageHeight = 600,  // Height of the output image in pixels
                LineScale = 0.5f   // Adjust this factor to make lines thicker or thinner
            };

            // Create and configure PenOptions
            PenOptions penOptions = new PenOptions
            {
                // Only StartCap and EndCap are supported; other pen attributes are not available
                StartCap = Aspose.CAD.LineCap.Flat,   // Flat start cap
                EndCap = Aspose.CAD.LineCap.Round     // Round end cap
            };

            // Assign the configured PenOptions to the rasterization options
            rasterOptions.PenOptions = penOptions;

            // Set up PNG export options and link the rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rendered image
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}