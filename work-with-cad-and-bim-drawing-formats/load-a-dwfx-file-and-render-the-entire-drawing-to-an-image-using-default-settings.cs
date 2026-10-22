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
            // Path to the source DWFx file
            string inputPath = "sample.dwfx";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "rendered.png");

            // Load the DWFx drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Default PNG export options
                PngOptions pngOptions = new PngOptions();

                // Render the entire drawing to an image file
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Rendering completed. Output saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}