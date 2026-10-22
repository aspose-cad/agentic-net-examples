using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Input DWG file path
        string inputPath = @"C:\CAD\sample.dwg";

        // Output PNG file path
        string outputPath = @"C:\CAD\output\sample.png";

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
                throw new FileNotFoundException($"Input file not found: {inputPath}");

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the DWG drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options to match AutoCAD on‑screen appearance
                var rasterOptions = new CadRasterizationOptions
                {
                    // Render all layouts (null means all)
                    Layouts = null,

                    // Use the drawing's original size; if you need a specific size, set PageWidth/PageHeight
                    // PageWidth = 1600f,
                    // PageHeight = 1200f,

                    // Preserve line weights, colors, hatches, etc.
                    // No additional properties are required for a faithful render
                };

                // Configure PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"DWG file rendered successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}