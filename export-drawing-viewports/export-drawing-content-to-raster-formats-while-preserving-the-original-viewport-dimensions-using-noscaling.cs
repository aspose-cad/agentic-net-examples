using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.png");

        // Validate input file
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
            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options to preserve original viewport (no scaling)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter; // correct enum member
                rasterOptions.LineScale = 1.0f;
                rasterOptions.AutomaticLayoutsScaling = false; // No scaling
                rasterOptions.PageWidth = (float)cadImage.Width;
                rasterOptions.PageHeight = (float)cadImage.Height;
                rasterOptions.Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save the image
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