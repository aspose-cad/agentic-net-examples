using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "result.png");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD file
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    UnitType = UnitType.Centimenter, // SDK spelling
                    LineScale = 1.0f
                };

                // Set pen options (only start/end caps are supported)
                var penOptions = new PenOptions
                {
                    StartCap = LineCap.Flat,
                    EndCap = LineCap.Flat
                };
                // Aspose.CAD does not expose dash style or dash pattern properties.
                // Therefore a specific dash style (e.g., dotted, dash‑dot) cannot be set.
                rasterOptions.PenOptions = penOptions;

                // Assign rasterization options to PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image as PNG
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}