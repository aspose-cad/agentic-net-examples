using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dxf";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            var cadImage = (CadImage)Image.Load(inputPath);

            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                Layouts = new[] { "Model" },
                Quality = new RasterizationQuality
                {
                    Text = RasterizationQualityValue.High,
                    Hatch = RasterizationQualityValue.High
                }
            };

            var pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Saved rasterized image to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}