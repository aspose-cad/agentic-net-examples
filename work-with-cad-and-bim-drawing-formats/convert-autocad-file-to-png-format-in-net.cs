using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input AutoCAD file path
            string inputPath = "input.dwg";

            // Prepare output directory and file path
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "output.png");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (optional, adjust as needed)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example dimensions; modify to suit requirements
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"File successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}