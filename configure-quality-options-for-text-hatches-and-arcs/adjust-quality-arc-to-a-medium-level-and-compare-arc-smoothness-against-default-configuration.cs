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
            string inputPath = "input.dwg";
            string defaultOutputPath = "output_default.png";
            
            string outputDir = Path.GetDirectoryName(defaultOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string mediumOutputPath = "output_medium.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // ---------- Default rasterization options ----------
                var defaultRasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new string[] { "Model" }
                };

                var defaultPngOptions = new PngOptions
                {
                    VectorRasterizationOptions = defaultRasterOptions
                };

                cadImage.Save(defaultOutputPath, defaultPngOptions);
                Console.WriteLine($"Default PNG saved to: {defaultOutputPath}");

                // ---------- Medium quality for arcs (if supported) ----------
                var mediumRasterOptions = new CadRasterizationOptions
                {
                    // The CadRasterizationQuality enum may not be available in the current library version.
                    // If it were, you would set it like this:
                    // Quality = CadRasterizationQuality.Medium,
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new string[] { "Model" }
                };

                var mediumPngOptions = new PngOptions
                {
                    VectorRasterizationOptions = mediumRasterOptions
                };

                cadImage.Save(mediumOutputPath, mediumPngOptions);
                Console.WriteLine($"Medium-quality PNG saved to: {mediumOutputPath}");

                // Simple comparison output (file size as a proxy for quality)
                var defaultInfo = new FileInfo(defaultOutputPath);
                var mediumInfo = new FileInfo(mediumOutputPath);
                Console.WriteLine($"Default PNG size: {defaultInfo.Length} bytes");
                Console.WriteLine($"Medium-quality PNG size: {mediumInfo.Length} bytes");
                Console.WriteLine("Compare the visual smoothness of arcs between the two images.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}