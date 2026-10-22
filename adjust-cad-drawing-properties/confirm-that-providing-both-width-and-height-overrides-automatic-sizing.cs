using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string sourcePath = "sample.dwg";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Load the CAD drawing with error handling
        Image cadImage;
        try
        {
            cadImage = Image.Load(sourcePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
            return;
        }

        using (cadImage)
        {
            // Configure PNG export options with explicit dimensions via rasterization options
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 800,
                PageHeight = 600,
                // Optional: set resolution if needed
                // Resolution = 300
            };

            var pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Path for the exported PNG file
            string outputPath = "output.png";

            // Save the CAD image as PNG with error handling
            try
            {
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving PNG file: {ex.Message}");
            }
        }
    }
}
