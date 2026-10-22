using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output directories
            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Path to the source DWG file
            string sourcePath = Path.Combine(dataDir, "sample.dwg");

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source DWG file not found at: " + sourcePath);
                return;
            }

            // Load the DWG drawing
            using (Image loadedImage = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)loadedImage;

                // Access and display layout names
                Console.WriteLine("Available Layouts:");
                foreach (var layoutEntry in cadImage.Layouts)
                {
                    Console.WriteLine(" - " + layoutEntry.Key);
                }

                // Prepare rasterization options with Zoom set to 1
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Zoom = 1
                };

                // Create PNG export options and assign rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rasterized image (border removal can be observed in the output)
                string outputPath = Path.Combine(outputDir, "rasterized_output.png");
                cadImage.Save(outputPath, pngOptions);

                Console.WriteLine("Rasterized image saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}