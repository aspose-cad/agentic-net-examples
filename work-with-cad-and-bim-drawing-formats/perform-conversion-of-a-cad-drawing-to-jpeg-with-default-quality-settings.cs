using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input CAD file path
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Output JPEG file path
            string outputPath = "output.jpg";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up rasterization options with high quality for text, hatch, and arc
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    }
                };

                // Configure JPEG options
                JpegOptions jpegOptions = new JpegOptions
                {
                    // In current Aspose.CAD versions the property is VectorRasterizationOptions
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}