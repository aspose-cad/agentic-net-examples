using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (replace with actual path)
                string sourcePath = "sample.dwg";
                // Output PNG file path
                string outputPath = "output.png";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Prepare rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Build a list of visible (not off and not frozen) layer names
                    List<string> visibleLayers = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        if (!isOff && !isFrozen)
                        {
                            visibleLayers.Add(layer.Name);
                        }
                    }

                    // If no layers are visible, skip export
                    if (visibleLayers.Count == 0)
                    {
                        Console.Error.WriteLine("No visible layers found. Export aborted.");
                        return;
                    }

                    rasterOptions.Layers = visibleLayers.ToArray();

                    // Set PNG save options with the configured rasterization options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rasterized image
                    image.Save(outputPath, pngOptions);
                    Console.WriteLine($"Export completed: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}