using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source DWG file
            string inputPath = "sample.dwg";

            // Verify that the source file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            // Create an output directory for the generated image
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "layout_image.png");

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                if (image is not CadImage cadImage)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Access and list layout names
                var layoutNames = cadImage.Layouts.Keys.ToArray();

                Console.WriteLine("Available layouts:");
                foreach (var name in layoutNames)
                {
                    Console.WriteLine($"- {name}");
                }

                // Choose the first layout for rendering (adjust as needed)
                string[] selectedLayouts = layoutNames.Length > 0 ? new[] { layoutNames[0] } : Array.Empty<string>();

                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    Layouts = selectedLayouts,
                    Zoom = 1,               // No scaling
                    PageWidth = 1600,       // Desired width
                    PageHeight = 1200       // Desired height
                };

                // Set PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}