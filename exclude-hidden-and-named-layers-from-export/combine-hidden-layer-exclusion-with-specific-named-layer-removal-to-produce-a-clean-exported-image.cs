using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

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

            // Load the CAD image
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Collect names of layers that are visible and not explicitly excluded
                List<string> layersToExport = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;                 // Off layers have negative ColorId
                    bool isFrozen = (layer.Flags & 1) != 0;         // Frozen flag is bit 0
                    bool isExcludedByName = string.Equals(layer.Name, "LayerToRemove", StringComparison.OrdinalIgnoreCase);

                    if (!isOff && !isFrozen && !isExcludedByName)
                    {
                        layersToExport.Add(layer.Name);
                    }
                }

                if (layersToExport.Count == 0)
                {
                    Console.WriteLine("No visible layers remain after filtering. Export aborted.");
                    return;
                }

                // Configure rasterization options to include only the selected layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = layersToExport.ToArray(),
                    PageWidth = 1600,
                    PageHeight = 1200
                    // DrawType can be set if needed, e.g., rasterOptions.DrawType = DrawTypeMode.UseObjectColor;
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the filtered image
                string outputPath = "output.png";
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Export completed. Image saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}