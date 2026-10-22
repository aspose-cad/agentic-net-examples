using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
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
                PngOptions pngOptions = new PngOptions();
                // Optional: configure rasterization options here
                // pngOptions.VectorRasterizationOptions = new CadRasterizationOptions
                // {
                //     PageWidth = 1600,
                //     PageHeight = 1200,
                //     DrawType = CadDrawTypeMode.UseObjectColor
                // };

                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine("Export succeeded.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export failed: {ex.Message}");
        }
    }
}