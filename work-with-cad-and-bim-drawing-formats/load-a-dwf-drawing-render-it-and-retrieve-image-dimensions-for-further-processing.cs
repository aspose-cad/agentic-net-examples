using Aspose.CAD.FileFormats.Cad;
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
            // Load the DWF drawing
            string sourcePath = "input.dwf";
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            using (Image cadImage = Image.Load(sourcePath))
            {
                // Retrieve image dimensions
                int imageWidth = cadImage.Width;
                int imageHeight = cadImage.Height;
                Console.WriteLine($"Image dimensions: {imageWidth}x{imageHeight}");

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };

                // Set up PNG output options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Prepare output path
                string outputPath = "output.png";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Render and save the image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Rendered image saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}