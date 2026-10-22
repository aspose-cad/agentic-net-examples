using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadPltRendering
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source PLT file
            string sourcePath = "input.plt";

            // Path for the rendered output image
            string outputPath = "output.png";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
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
                // Load the PLT file
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Configure rasterization options for higher quality rendering
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 2000,
                        PageHeight = 2000,
                        // Set high quality for text, hatch, and arcs
                        Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High,
                            Arc = RasterizationQualityValue.High
                        }
                    };

                    // Create PNG save options and attach the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine("Rendering completed. Output saved to: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}