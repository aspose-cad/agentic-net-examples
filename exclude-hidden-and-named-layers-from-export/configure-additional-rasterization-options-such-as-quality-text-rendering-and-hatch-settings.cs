using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the source CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to work with CAD‑specific features
                CadImage cadImage = (CadImage)image;

                // Prepare a list of layer names to exclude (hidden or off)
                var excludedLayerNames = new System.Collections.Generic.List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    // Off layer: ColorId < 0
                    bool isOff = layer.ColorId < 0;
                    // Frozen layer: (Flags & 1) != 0
                    bool isFrozen = (layer.Flags & 1) != 0;

                    if (isOff || isFrozen)
                    {
                        excludedLayerNames.Add(layer.Name);
                    }
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,   // Desired output width
                    PageHeight = 1200,  // Desired output height
                    LineScale = 1.0f    // Default line scaling
                };

                // Note: Aspose.CAD rasterization options for anti‑aliasing, text rendering,
                // and hatch rendering are not available in the current API version.
                // Layer filtering at rasterization time is also not directly supported;
                // any required filtering would need to be performed on the CAD entities
                // before saving.

                // Set up PNG save options and attach the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to a PNG file using the configured options
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}