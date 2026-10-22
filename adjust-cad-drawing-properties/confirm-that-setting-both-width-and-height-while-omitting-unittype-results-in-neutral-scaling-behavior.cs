using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage if CAD-specific members are needed later
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options: set both width and height, omit UnitType
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,   // Desired output width in pixels
                    PageHeight = 600   // Desired output height in pixels
                    // UnitType is not set – neutral scaling will be applied based on pixel dimensions
                };

                // Create PNG export options and assign the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing to PNG using the configured options
                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Export completed with neutral scaling (Width and Height set, UnitType omitted). Output saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"Error saving file: {saveEx.Message}");
                }
            }
        }
        catch (Exception loadEx)
        {
            Console.WriteLine($"Error loading CAD file: {loadEx.Message}");
        }
    }
}
