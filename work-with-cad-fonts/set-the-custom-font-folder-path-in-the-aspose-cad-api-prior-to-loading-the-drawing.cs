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
            // Paths – replace with actual locations
            string inputPath = "input.dwg";
            string outputPath = "output.png";
            string customFontFolder = @"C:\MyFonts";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Set custom font folder before any rendering or processing
                cadImage.DefaultFont = customFontFolder;

                // Render the drawing to PNG (example)
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Additional rasterization settings can be configured here
                    }
                };

                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Rendered image saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}