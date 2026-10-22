using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Save as JPEG
                JpegOptions jpegOptions = new JpegOptions();
                string jpegPath = Path.Combine(outputDir, "output.jpg");
                cadImage.Save(jpegPath, jpegOptions);

                // Save as PNG
                PngOptions pngOptions = new PngOptions();
                string pngPath = Path.Combine(outputDir, "output.png");
                cadImage.Save(pngPath, pngOptions);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}