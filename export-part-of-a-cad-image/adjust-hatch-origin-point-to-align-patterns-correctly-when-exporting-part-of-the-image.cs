using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadHatchOriginExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                        // Note: HatchPatternOrigin property is not available in the current API.
                    };

                    // Set PNG export options and attach the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the exported image
                    cadImage.Save(outputPath, pngOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}