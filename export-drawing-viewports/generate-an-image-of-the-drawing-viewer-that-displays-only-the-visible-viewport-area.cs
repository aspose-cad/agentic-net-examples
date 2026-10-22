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
                // Define input and output directories
                string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);

                // Path to the source CAD file
                string sourcePath = Path.Combine(dataDir, "sample.dwg");

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("Source CAD file not found: " + sourcePath);
                    return;
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members if needed
                    CadImage cad = (CadImage)cadImage;

                    // Configure rasterization options to capture only the visible viewport area
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 800;   // Width of the viewport in pixels
                    rasterOptions.PageHeight = 600;  // Height of the viewport in pixels
                    // Optionally specify which layout to render (e.g., Model space)
                    rasterOptions.Layouts = new string[] { "Model" };

                    // Set up PNG export options and attach the rasterization settings
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the viewport image
                    string outputPath = Path.Combine(outputDir, "viewport.png");
                    cad.Save(outputPath, pngOptions);

                    Console.WriteLine("Viewport image saved to: " + outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}