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
                // Path to the source CAD file
                string inputPath = "sample.dwg";

                // Verify that the source file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' not found.");
                    return;
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific properties if needed
                    CadImage cadImg = cadImage as CadImage;

                    // Configure rasterization to use the exact viewport boundaries (no scaling)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.NoScaling = true;                     // Preserve exact viewport size
                    rasterOptions.PageWidth = cadImg.Width;             // Match page width to drawing width
                    rasterOptions.PageHeight = cadImg.Height;           // Match page height to drawing height

                    // Set PNG output options and attach the rasterization settings
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Create an output directory for the result
                    string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                    Directory.CreateDirectory(outputDir);
                    string outputPath = Path.Combine(outputDir, "output.png");

                    // Save the rasterized image
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Raster image saved to: {outputPath}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}