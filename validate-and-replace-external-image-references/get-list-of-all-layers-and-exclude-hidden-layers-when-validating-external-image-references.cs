using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Gather names of visible layers (not off and not frozen)
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

                // Iterate over top‑level entities and validate external image references on visible layers
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    string entityLayerName = entity.LayerName;

                    // Skip entities that belong to hidden layers
                    if (!visibleLayerNames.Contains(entityLayerName))
                        continue;

                    // Check for external image references (CadRasterImage)
                    if (entity is CadRasterImage rasterImage)
                    {
                        Console.WriteLine($"External image reference found on visible layer '{entityLayerName}'.");
                        // Insert validation logic here (e.g., file existence checks)
                        // Example placeholder:
                        // string imagePath = rasterImage.ImageDefReference;
                        // if (!File.Exists(imagePath)) { /* handle missing image */ }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}