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
            // Define input DWG file path and output PNG file path
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWG drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PNG export options
                PngOptions pngOptions = new PngOptions();

                // Save the drawing as a PNG raster image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"DWG file successfully exported to PNG: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}