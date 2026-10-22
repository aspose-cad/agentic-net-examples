using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageHeight = 1000,
                    UnitType = UnitType.Millimeter
                    // PageWidth is intentionally left unset to be auto‑calculated
                };

                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving image: {ex.Message}");
                    return;
                }

                Console.WriteLine($"Calculated Width (PageWidth): {rasterOptions.PageWidth}");
                Console.WriteLine($"Used Height (PageHeight): {rasterOptions.PageHeight}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD image: {ex.Message}");
        }
    }
}
