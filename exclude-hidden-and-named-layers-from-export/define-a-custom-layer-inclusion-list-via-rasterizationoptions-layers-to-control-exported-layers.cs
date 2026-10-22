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
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members (e.g., layers)
                CadImage cadImage = (CadImage)image;

                // Build a list of layers that are visible (not off) and not frozen
                List<string> includedLayers = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;               // Off layers have negative ColorId
                    bool isFrozen = (layer.Flags & 1) != 0;       // Frozen flag is bit 0
                    if (!isOff && !isFrozen && !string.IsNullOrEmpty(layer.Name))
                    {
                        includedLayers.Add(layer.Name);
                    }
                }

                if (includedLayers.Count == 0)
                {
                    Console.Error.WriteLine("No visible layers found to export.");
                    return;
                }

                // Prepare rasterization options and specify which layers to include in the export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" },
                    Layers = includedLayers.ToArray()
                };

                // Set up PNG export options and attach the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the filtered drawing to a PNG file
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