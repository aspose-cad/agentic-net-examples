using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.png");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(outputDir);

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization to disable automatic layout scaling
                var rasterOptions = new CadRasterizationOptions
                {
                    AutomaticLayoutsScaling = false,          // Disable scaling
                    UnitType = UnitType.Centimenter,           // Correct enum value
                    PageWidth = 200,                           // Width of viewport (centimenters)
                    PageHeight = 200,                          // Height of viewport (centimenters)
                    LineScale = 1.0f                           // Optional line scaling
                };

                // Set PNG export options and attach rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image using the defined viewport
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Image saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}