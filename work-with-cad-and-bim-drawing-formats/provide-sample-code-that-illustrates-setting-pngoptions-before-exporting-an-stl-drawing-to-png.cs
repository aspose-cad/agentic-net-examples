using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "sample.stl";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "sample.png");

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
                throw new FileNotFoundException($"Input file not found: {inputPath}");

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the STL drawing
            Image cadImage = Image.Load(inputPath);

            // Set up rasterization options for PNG export
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter,   // SDK spelling
                LineScale = 1.0f,
                PageWidth = cadImage.Width,
                PageHeight = cadImage.Height,
                AutomaticLayoutsScaling = true
            };

            // Configure PNG export options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export to PNG
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}