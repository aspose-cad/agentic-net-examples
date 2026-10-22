using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadFontCleanupSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file (replace with actual path)
            string sourceCadPath = "input.dwg";

            // Verify that the source CAD file exists
            if (!File.Exists(sourceCadPath))
            {
                Console.WriteLine($"Source CAD file not found: {sourceCadPath}");
                return;
            }

            // Create an output directory for generated files
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);

            // List of temporary font files that will be used during processing
            // (In a real scenario these could be extracted from a zip, downloaded, etc.)
            List<string> temporaryFontFiles = new List<string>
            {
                Path.Combine(Path.GetTempPath(), "tempFont1.ttf"),
                Path.Combine(Path.GetTempPath(), "tempFont2.ttf")
            };

            // Example: create dummy temporary font files (for demonstration purposes)
            foreach (string fontPath in temporaryFontFiles)
            {
                if (!File.Exists(fontPath))
                {
                    // Create an empty file to simulate a temporary font
                    using (FileStream fs = File.Create(fontPath))
                    {
                        // No content needed for the placeholder
                    }
                }
            }

            // Load the CAD image
            using (Image image = Image.Load(sourceCadPath))
            {
                // Cast to CadImage to access CAD-specific properties
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Optionally set a default font from the temporary fonts (first one in the list)
                if (temporaryFontFiles.Count > 0 && File.Exists(temporaryFontFiles[0]))
                {
                    cadImage.DefaultFont = temporaryFontFiles[0];
                }

                // Prepare rasterization options for rendering
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" } // Render the Model layout; adjust as needed
                };

                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Define output PNG path
                string outputPngPath = Path.Combine(outputDir, "rendered.png");

                // Save the rendered image
                cadImage.Save(outputPngPath, pngOptions);
                Console.WriteLine($"Rendered image saved to: {outputPngPath}");
            }

            // Clean up temporary font files
            foreach (string fontPath in temporaryFontFiles)
            {
                try
                {
                    if (File.Exists(fontPath))
                    {
                        File.Delete(fontPath);
                        Console.WriteLine($"Deleted temporary font file: {fontPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete temporary font file '{fontPath}': {ex.Message}");
                }
            }

            Console.WriteLine("Processing completed.");
        }
    }
}