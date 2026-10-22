using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string outputPath = Path.Combine(outputDir, "output.png");

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Substitute missing fonts with a default font
                cadImage.DefaultFont = "Arial";

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = cadImage.Layouts.Keys.ToArray()
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
            }

            // Verify that the image was created successfully
            if (File.Exists(outputPath) && new FileInfo(outputPath).Length > 0)
            {
                Console.WriteLine($"Image saved successfully: {outputPath}");
            }
            else
            {
                Console.Error.WriteLine("Failed to create output image.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}