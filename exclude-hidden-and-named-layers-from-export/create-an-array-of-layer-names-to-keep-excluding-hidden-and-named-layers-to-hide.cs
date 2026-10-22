using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path to the output PNG file
                string outputPath = "output.png";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // List of layer names that should be hidden regardless of their visibility flags
                List<string> layersToHide = new List<string> { "LayerToHide1", "LayerToHide2" };

                // Load the CAD image
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Collect names of layers that are visible and not in the hide list
                    List<string> visibleLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0; // Bit 0 indicates frozen
                        bool shouldHideByName = layersToHide.Contains(layer.Name);

                        if (!isOff && !isFrozen && !shouldHideByName)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }

                    // If no layers are visible, skip exporting
                    if (visibleLayerNames.Count == 0)
                    {
                        Console.WriteLine("No visible layers to export.");
                        return;
                    }

                    // Set up rasterization options with the filtered layer list
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layers = visibleLayerNames.ToArray()
                    };

                    // Set up PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the filtered drawing to PNG
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Exported visible layers to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}