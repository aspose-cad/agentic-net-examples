using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
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
                    PageWidth = 1200,   // Desired image width in pixels
                    PageHeight = 800    // Desired image height in pixels
                };

                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
