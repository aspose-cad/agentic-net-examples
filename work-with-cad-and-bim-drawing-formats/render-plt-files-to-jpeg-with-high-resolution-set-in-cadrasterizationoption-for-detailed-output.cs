using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class PltToJpegRenderer
{
    static void Main()
    {
        // Input PLT file path
        string inputFile = @"input.plt";

        // Output directory and file path
        string outputDirectory = @"output";
        string outputFile = Path.Combine(outputDirectory, "rendered.jpg");

        // Validate input file existence
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Load the PLT file as a CadImage
            using (Image cadImage = Image.Load(inputFile))
            {
                // Configure high‑quality rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    // Set a large page size for high‑resolution output (pixels)
                    PageWidth = 2000,
                    PageHeight = 2000,
                    UnitType = UnitType.Pixel,
                    // High quality for text, hatch patterns and arcs
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    }
                };

                // Configure JPEG output options
                var jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image as JPEG
                cadImage.Save(outputFile, jpegOptions);
                Console.WriteLine($"Rendering completed: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during rendering: {ex.Message}");
        }
    }
}