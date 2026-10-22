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

            // Ensure output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
                Directory.CreateDirectory(outDir);

            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options for higher resolution
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 2000,
                PageHeight = 2000
                // Other properties remain at defaults
            };

            // Set PNG export options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rendered image
            cadImage.Save(outputPath, pngOptions);

            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}