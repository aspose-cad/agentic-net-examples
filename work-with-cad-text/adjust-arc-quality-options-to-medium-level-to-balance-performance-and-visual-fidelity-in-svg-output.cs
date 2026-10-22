using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.svg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                var raster = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.Medium
                    }
                };

                var svgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = raster
                };

                cadImage.Save(outputPath, svgOptions);
                Console.WriteLine($"SVG saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}