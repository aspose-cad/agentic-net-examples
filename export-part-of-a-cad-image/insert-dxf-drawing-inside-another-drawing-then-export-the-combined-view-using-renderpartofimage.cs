using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string inputDir = Path.Combine(Directory.GetCurrentDirectory(), "Input");
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            string basePath = Path.Combine(inputDir, "base.dwg");
            string overlayPath = Path.Combine(inputDir, "overlay.dxf");

            // Verify input files exist
            if (!File.Exists(basePath))
            {
                Console.Error.WriteLine($"Base drawing not found: {basePath}");
                return;
            }
            if (!File.Exists(overlayPath))
            {
                Console.Error.WriteLine($"Overlay drawing not found: {overlayPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load both drawings
            using (Image baseImg = Image.Load(basePath))
            using (Image overlayImg = Image.Load(overlayPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage baseCad = baseImg as CadImage;
                CadImage overlayCad = overlayImg as CadImage;

                if (baseCad == null || overlayCad == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded images to CadImage.");
                    return;
                }

                // Insert all entities from the overlay drawing into the base drawing
                baseCad.Entities = baseCad.Entities.Concat(overlayCad.Entities).ToArray();

                // Prepare rasterization options for rendering
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = (int)baseCad.Size.Width,
                    PageHeight = (int)baseCad.Size.Height,
                    // Optional: set background color, line weight, etc.
                };

                // Prepare PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the combined drawing as PNG
                string outputPath = Path.Combine(outputDir, "combined.png");
                baseCad.Save(outputPath, pngOptions);
                Console.WriteLine($"Combined drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}