using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.plt";
            string outputPath = "output.jpg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 200,
                    PageHeight = 200,
                    AutomaticLayoutsScaling = true,
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.Low,
                        Hatch = RasterizationQualityValue.Low,
                        Arc = RasterizationQualityValue.Low
                    }
                };

                var jpegOptions = new JpegOptions
                {
                    Quality = 75,
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"Conversion completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}