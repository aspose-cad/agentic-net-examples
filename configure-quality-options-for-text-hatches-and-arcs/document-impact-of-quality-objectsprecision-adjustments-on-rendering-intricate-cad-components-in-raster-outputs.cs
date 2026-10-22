using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define paths
        string dataDir = Path.Combine(Environment.CurrentDirectory, "Data");
        string inputPath = Path.Combine(dataDir, "sample.dwg");
        string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
        string outputPath = Path.Combine(outputDir, "output.png");

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
            // Load CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;          // Width of the raster page
                rasterOptions.PageHeight = 1200;         // Height of the raster page
                rasterOptions.UnitType = UnitType.Millimeter;
                // Adjust line scale to influence rendering of fine details.
                // Lower values produce finer lines, higher values produce thicker lines.
                rasterOptions.LineScale = 0.5f;

                // NOTE: The ObjectsPrecision property does not exist in the current SDK.
                // Rendering quality can be influenced via LineScale, DPI, or other raster options.

                // Set PNG export options
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save rasterized image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Raster image saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}