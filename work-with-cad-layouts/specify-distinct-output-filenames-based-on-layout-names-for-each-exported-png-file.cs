using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayoutExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "sample.dwg";

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("Source CAD file not found: " + sourcePath);
                    return;
                }

                // Create an output directory for the exported PNG files
                string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDirectory);

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Iterate through each layout in the CAD drawing
                    foreach (CadLayout layout in cadImage.Layouts.Values)
                    {
                        // Get the layout name
                        string layoutName = layout.LayoutName;

                        // Build a distinct output file name based on the layout name
                        string outputPath = Path.Combine(outputDirectory, layoutName + ".png");

                        // Configure rasterization options for the specific layout
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.Layouts = new string[] { layoutName };

                        // Set PNG export options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Export the layout to PNG
                        cadImage.Save(outputPath, pngOptions);

                        Console.WriteLine($"Exported layout '{layoutName}' to '{outputPath}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}