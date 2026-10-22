using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace ConfigureQualityOptions
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths (adjust as needed)
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            try
            {
                // Verify that the input CAD file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options with high quality for both text and hatches
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Set the desired quality settings
                        Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High
                        }
                    };

                    // Set up PNG save options and attach the rasterization options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image as PNG
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Image saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}