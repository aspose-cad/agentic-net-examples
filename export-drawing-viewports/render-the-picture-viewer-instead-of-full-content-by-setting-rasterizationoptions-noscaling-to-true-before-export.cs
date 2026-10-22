using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

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
        // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage for CAD-specific operations
                    CadImage cadImage = (CadImage)image;

                    // Configure rasterization options with NoScaling set to true
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.NoScaling = true;
                    rasterOptions.PageWidth = 1600;   // optional: set desired width
                    rasterOptions.PageHeight = 1200;  // optional: set desired height

                    // Set up PNG export options and assign the rasterization options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Path for the exported PNG file
                    string outputPath = "output.png";

                    // Export the CAD drawing to PNG using the configured options
                    cadImage.Save(outputPath, pngOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}