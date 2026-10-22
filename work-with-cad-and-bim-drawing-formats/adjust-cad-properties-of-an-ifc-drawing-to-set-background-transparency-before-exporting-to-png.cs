using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        // Paths for input IFC and output PNG
        string inputPath = "input.ifc";
        string outputPath = "output.png";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the IFC drawing
            using (var cadImage = Image.Load(inputPath))
            {
                // Configure rasterization with a fully transparent background
                var rasterOptions = new CadRasterizationOptions
                {
                    BackgroundColor = Color.FromArgb(0, 0, 0, 0), // Alpha = 0 for transparency
                    EmbedBackground = true
                };

                // Set PNG export options using the rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the drawing to PNG with transparent background
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"PNG exported successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}