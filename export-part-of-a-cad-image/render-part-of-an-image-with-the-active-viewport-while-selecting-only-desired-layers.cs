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
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            Image cadImage = Image.Load(inputPath);

            // Set up rasterization options to select specific layers
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                Layers = new string[] { "Layer1", "Layer2" }
            };

            // Configure PNG save options with the rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rendered portion to a PNG file
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}