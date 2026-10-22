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
            string inputPath = "input.stl";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the STL file
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600f,               // Desired width in pixels
                PageHeight = 1200f,              // Desired height in pixels
                Layouts = new string[] { "Model" } // STL typically has a single layout
                // Additional properties can be set here as needed
            };

            // Set PNG save options and attach rasterization options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rasterized image
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Rasterized image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}