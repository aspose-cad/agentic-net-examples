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
        const string outputPath = "output.png";

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
            Image cadImage = Image.Load(inputPath);

            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                Quality = new RasterizationQuality
                {
                    Text = RasterizationQualityValue.High,
                    Hatch = RasterizationQualityValue.High,
                    Arc = RasterizationQualityValue.High
                },
                PageWidth = 1600,
                PageHeight = 1200
            };

            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}