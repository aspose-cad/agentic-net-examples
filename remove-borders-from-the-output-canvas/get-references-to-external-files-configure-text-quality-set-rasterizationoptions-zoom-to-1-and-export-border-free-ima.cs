using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputPath = @"C:\Input\sample.dwg";
            // Output PNG file path
            string outputPath = @"C:\Output\sample.png";

            // Validate input file
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file does not exist: {inputPath}");
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
                // Load CAD image
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter, // SDK's actual spelling
                    LineScale = 1.0f,
                    Zoom = 1.0f,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image
                cadImage.Save(outputPath, pngOptions);

                Console.WriteLine($"Image exported successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}