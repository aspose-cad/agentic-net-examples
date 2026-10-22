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
                // Prepare input and output directories
                string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);

                // Path to the source CAD file
                string sourcePath = Path.Combine(dataDir, "sample.dwg");
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("Source CAD file not found: " + sourcePath);
                    return;
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Configure rasterization options to match the original viewport size
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    // Example dimensions – replace with the actual viewport size if known
                    rasterOptions.PageWidth = 800;
                    rasterOptions.PageHeight = 600;
                    // If the drawing contains multiple layouts, specify the desired one
                    rasterOptions.Layouts = new string[] { "Model" };

                    // Set up PNG export options and attach rasterization settings
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the raster image
                    string outPath = Path.Combine(outputDir, "output.png");
                    cadImage.Save(outPath, pngOptions);
                    Console.WriteLine("Raster image saved to: " + outPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}