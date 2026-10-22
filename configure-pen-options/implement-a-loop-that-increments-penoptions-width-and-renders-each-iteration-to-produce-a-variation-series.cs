using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace ConfigurePenOptions
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputFile = @"C:\Data\sample.dwg";

            // Output directory
            string outputDir = @"C:\Data\Output";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputFile))
                {
                    // PenOptions does not expose a Width property. To vary line thickness we adjust LineScale.
                    for (int i = 1; i <= 5; i++)
                    {
                        // Create rasterization options
                        var rasterOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            UnitType = UnitType.Centimenter,
                            // LineScale controls the exported line thickness.
                            LineScale = 0.5f * i
                        };

                        // Only start/end caps are configurable.
                        rasterOptions.PenOptions.StartCap = LineCap.Round;
                        rasterOptions.PenOptions.EndCap = LineCap.Round;

                        // Create PNG export options and assign the rasterization options
                        var pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Build output file name
                        string outputPath = Path.Combine(outputDir, $"sample_variant_{i}.png");

                        // Save the image
                        cadImage.Save(outputPath, pngOptions);

                        Console.WriteLine($"Saved variant {i} to {outputPath} (LineScale = {rasterOptions.LineScale})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}