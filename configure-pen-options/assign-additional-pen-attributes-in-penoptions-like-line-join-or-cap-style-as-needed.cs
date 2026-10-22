using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input CAD file path
        string inputPath = "input.dwg";

        // Desired output PNG file path
        string outputPath = "output.png";

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

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options, including pen caps
                var rasterOptions = new CadRasterizationOptions
                {
                    // Unit type spelling as defined in the SDK
                    UnitType = UnitType.Centimenter,
                    // Controls exported line thickness
                    LineScale = 1.0f,
                    // PenOptions supports only StartCap and EndCap
                    PenOptions = new PenOptions
                    {
                        StartCap = LineCap.Flat,   // Example start cap
                        EndCap   = LineCap.Round   // Example end cap
                    }
                };

                // Set up PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image as PNG
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"PNG saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}