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
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            Image image = Image.Load(inputPath);
            CadImage cadImage = image as CadImage;
            if (cadImage == null)
            {
                Console.Error.WriteLine("Failed to load CAD image.");
                return;
            }

            // Substitute missing fonts with a known font
            cadImage.DefaultFont = "Arial";

            // Configure rasterization options for image export
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200
                // BackgroundColor defaults to white; omitted to avoid System.Drawing usage
            };

            // Set PNG export options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export CAD drawing to PNG image
            cadImage.Save(outputPath, pngOptions);

            Console.WriteLine($"CAD drawing exported successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}