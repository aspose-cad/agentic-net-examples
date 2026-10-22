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
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string inputPath = Path.Combine(dataDir, "sample.dwg");
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output.png");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization to match the exact viewport boundaries
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Setting PageWidth and PageHeight to 0 lets Aspose.CAD use the drawing's original extents
                    PageWidth = 0,
                    PageHeight = 0
                };

                // Set PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the raster image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Raster image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}