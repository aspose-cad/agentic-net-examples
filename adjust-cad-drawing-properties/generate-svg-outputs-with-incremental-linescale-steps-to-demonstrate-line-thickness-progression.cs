using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input CAD file path (replace with actual path)
        string inputPath = "input.dwg";

        // Output directory for SVG files
        string outputDir = "output";

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

        // Define line scale factors to demonstrate thickness progression
        float[] scaleFactors = { 0.5f, 1.0f, 2.0f };

        foreach (float factor in scaleFactors)
        {
            try
            {
                // Load CAD image
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Configure rasterization options with line scale factor
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        LineScale = factor,
                        PageWidth = 800f,
                        PageHeight = 600f,
                        UnitType = UnitType.Centimenter
                    };

                    // Set SVG save options
                    SvgOptions svgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output file path
                    string outputPath = Path.Combine(outputDir, $"output_scale_{factor}.svg");

                    // Save as SVG
                    cadImage.Save(outputPath, svgOptions);
                    Console.WriteLine($"Saved SVG with line scale {factor} to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing line scale {factor}: {ex.Message}");
            }
        }
    }
}
