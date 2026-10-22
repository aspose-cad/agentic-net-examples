using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.jpg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DWG drawing
            Image cadImage = Image.Load(inputPath);

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

            // Configure JPEG save options
            JpegOptions jpegOptions = new JpegOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export the drawing to JPEG
            cadImage.Save(outputPath, jpegOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}