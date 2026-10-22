using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class AdjustArcQualityExample
{
    static void Main()
    {
        string inputPath = "input.dfx";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Set rasterization options with high arc quality
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                Quality = new RasterizationQuality
                {
                    Arc = RasterizationQualityValue.High
                }
            };

            // Prepare PNG export options using the rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the rendered image
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}