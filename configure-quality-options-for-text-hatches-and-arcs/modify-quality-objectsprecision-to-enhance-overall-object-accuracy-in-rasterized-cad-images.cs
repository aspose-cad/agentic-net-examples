using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadRasterizationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputPath = "input.dwg";
            // Output raster image path
            string outputPath = "output.png";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                // Load CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        UnitType = UnitType.Millimeter,
                        // Increase line scale to improve overall object precision
                        LineScale = 0.5f
                    };

                    // Set PNG save options with the rasterization settings
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save rasterized image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"CAD file successfully rasterized to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}