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
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PNG export options
                PngOptions pngOptions = new PngOptions();

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,   // Desired output width
                    PageHeight = 1200   // Desired output height
                    // Additional quality improvements can be controlled via other available properties if needed
                };

                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Path for the exported PNG file
                string outputPath = "output.png";

                // Save the CAD drawing as a PNG image
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}