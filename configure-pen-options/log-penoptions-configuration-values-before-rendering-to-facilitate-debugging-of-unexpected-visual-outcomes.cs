using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string dataDir = "Data";
            string inputFileName = "sample.dwg";
            string inputPath = Path.Combine(dataDir, inputFileName);

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string outputFileName = "sample_pen.png";
            string outputPath = Path.Combine(outputDir, outputFileName);

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage for CAD‑specific options
                CadImage cadImage = (CadImage)image;

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                    // No custom PenOptions are set; defaults will be used.
                };

                // Log rasterization (including PenOptions) configuration before rendering
                Console.WriteLine("Rasterization Options:");
                Console.WriteLine($"  PageWidth: {rasterOptions.PageWidth}");
                Console.WriteLine($"  PageHeight: {rasterOptions.PageHeight}");
                if (rasterOptions.PenOptions != null)
                {
                    Console.WriteLine("  PenOptions are configured.");
                }
                else
                {
                    Console.WriteLine("  PenOptions are not configured (using defaults).");
                }

                // Set up PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rasterized image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Image saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}