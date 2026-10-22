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
            string inputPath = "sample.dwf";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "preview.png");

            using (Image cadImage = Image.Load(inputPath))
            {
                // Default rendering options for quick preview of the entire drawing
                PngOptions pngOptions = new PngOptions();

                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Preview image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}