using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Input CAD file path
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output raster image path
                string outputPath = "output.png";

                // Load the CAD file
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set up rasterization options
                    CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions();
                    rasterizationOptions.Zoom = 1; // Remove borders from the result canvas

                    // Configure PNG export options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterizationOptions;

                    // Export the CAD file to PNG
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