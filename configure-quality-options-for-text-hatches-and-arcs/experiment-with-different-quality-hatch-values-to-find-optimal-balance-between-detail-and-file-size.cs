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
            // Placeholder input CAD file path
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Different hatch quality enum values to test
            var hatchQualities = new[]
            {
                RasterizationQualityValue.Low,
                RasterizationQualityValue.Medium,
                RasterizationQualityValue.High
            };

            foreach (var hatchQuality in hatchQualities)
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set up rasterization options with the current hatch quality
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { "Model" }
                    };
                    rasterOptions.Quality.Hatch = hatchQuality;

                    // Set up PDF export options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputFileName = $"output_hatch_{hatchQuality}.pdf";
                    string outputPath = Path.Combine(outputDir, outputFileName);

                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Saved {outputPath} with Hatch quality {hatchQuality}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}