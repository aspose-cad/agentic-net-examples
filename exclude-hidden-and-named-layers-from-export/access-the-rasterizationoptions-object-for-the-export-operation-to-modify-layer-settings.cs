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
            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Create rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                // Filter layers: include only layers that are not off (ColorId >= 0) and not frozen (Flags bit 0 == 0)
                List<string> visibleLayerNames = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    if (!isOff && !isFrozen)
                    {
                        visibleLayerNames.Add(layer.Name);
                    }
                }

                if (visibleLayerNames.Count == 0)
                {
                    Console.WriteLine("No visible layers found. Export aborted.");
                    return;
                }

                // Assign the filtered layer names to the rasterization options
                rasterOptions.Layers = visibleLayerNames.ToArray();

                // Set up PNG export options and assign the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the exported image
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