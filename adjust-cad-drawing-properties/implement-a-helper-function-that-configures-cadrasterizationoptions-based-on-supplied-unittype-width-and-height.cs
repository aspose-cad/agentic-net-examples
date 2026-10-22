using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadRasterizationOptions rasterOptions = ConfigureRasterizationOptions("Millimeter", 800, 600);
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"Error saving image: {saveEx.Message}");
                }
            }
        }
        catch (Exception loadEx)
        {
            Console.WriteLine($"Error loading image: {loadEx.Message}");
        }
    }

    static CadRasterizationOptions ConfigureRasterizationOptions(string unitType, int width, int height)
    {
        CadRasterizationOptions options = new CadRasterizationOptions();

        if (Enum.TryParse(unitType, out Aspose.CAD.ImageOptions.UnitType parsedUnit))
        {
            options.UnitType = parsedUnit;
        }
        else
        {
            Console.WriteLine($"Invalid unit type '{unitType}'. Using default.");
        }

        options.PageWidth = width;
        options.PageHeight = height;

        return options;
    }
}
