using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        // Paths for input CAD file and output image
        string inputPath = "sample.dwg";
        string outputPath = "output.png";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Load the CAD drawing
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                Zoom = 1,                         // Preserve original scale
                UnitType = UnitType.Centimenter,  // Use centimeters for measurements
                LineScale = 1.0f                  // Default line scaling
            };

            // Remove canvas borders by setting zero margins
            Margins zeroMargins = new Margins
            {
                Left = 0,
                Right = 0,
                Top = 0,
                Bottom = 0
            };
            rasterOptions.Margins = zeroMargins;

            // Set up PNG export options with the rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export the CAD drawing to PNG while preserving hatch quality
            cadImage.Save(outputPath, pngOptions);

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during export: {ex.Message}");
        }
    }
}