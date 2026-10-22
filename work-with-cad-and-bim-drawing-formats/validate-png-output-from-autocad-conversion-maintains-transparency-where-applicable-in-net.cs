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
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string outputPath = Path.Combine(outputDir, "output.png");

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create PNG options. The Aspose.CAD API does not expose a BackgroundColor property,
                // so explicit background color configuration (e.g., setting transparency) is not available.
                // The conversion will preserve any inherent transparency in the source drawing if supported.
                PngOptions pngOptions = new PngOptions();

                // Save the image as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"PNG file saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}