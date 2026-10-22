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
            // Define input and output paths
            string inputPath = @"Input\model.dwg"; // placeholder path for a 3D CAD file
            string outputDir = @"Output";
            string outputPath = Path.Combine(outputDir, "TopView.png");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Prepare PNG export options with rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1000,
                        PageHeight = 1000
                    }
                };

                // Save the image (default view is top-down for many CAD files)
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Top view image saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}