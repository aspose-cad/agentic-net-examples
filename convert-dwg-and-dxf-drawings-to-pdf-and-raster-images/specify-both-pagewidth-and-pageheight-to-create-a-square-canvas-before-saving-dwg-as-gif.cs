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
            // Define input and output paths
            string inputPath = "input.dwg";
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "output.gif");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the DWG file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options to create a square canvas
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1000,   // Width in pixels
                    PageHeight = 1000   // Height in pixels (same as width for a square)
                };

                // Set up GIF save options
                GifOptions saveOptions = new GifOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image as GIF
                cadImage.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"DWG file successfully saved as GIF to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}