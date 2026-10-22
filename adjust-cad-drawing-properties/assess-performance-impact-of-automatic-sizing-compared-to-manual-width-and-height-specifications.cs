using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        try
        {
            // Verify input file existence
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load CAD image
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Configure rasterization options (manual width and height)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                };

                // Set PNG save options with the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
