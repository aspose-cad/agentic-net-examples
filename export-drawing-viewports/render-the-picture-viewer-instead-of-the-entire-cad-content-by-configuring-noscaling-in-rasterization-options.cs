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
            string inputFile = "sample.dwg";
            string outputFolder = "Output";
            string outputFile = Path.Combine(outputFolder, "rendered.png");

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputFile))
            {
                // Configure rasterization options to render only the picture viewer (no scaling)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    NoScaling = true
                };

                // Set PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputFile, pngOptions);
            }

            Console.WriteLine($"Rendering completed. Output saved to: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}