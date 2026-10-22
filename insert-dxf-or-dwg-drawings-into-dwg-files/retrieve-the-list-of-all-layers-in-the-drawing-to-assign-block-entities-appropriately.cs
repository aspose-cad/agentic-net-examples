using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Collect distinct layer names from all entities
                var layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var entity in cadImage.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                        layerNames.Add(entity.LayerName);
                }

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (var entity in block.Entities)
                    {
                        if (!string.IsNullOrEmpty(entity.LayerName))
                            layerNames.Add(entity.LayerName);
                    }
                }

                Console.WriteLine("Layers in the drawing:");
                foreach (var name in layerNames)
                {
                    Console.WriteLine("- " + name);
                }

                // Assign all entities to the first layer if any layer exists
                if (layerNames.Count > 0)
                {
                    string targetLayer = null;
                    foreach (var name in layerNames)
                    {
                        targetLayer = name;
                        break;
                    }

                    foreach (var entity in cadImage.Entities)
                    {
                        entity.LayerName = targetLayer;
                    }

                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (var entity in block.Entities)
                        {
                            entity.LayerName = targetLayer;
                        }
                    }
                }

                // Save the modified drawing using DWG options
                var saveOptions = new DwgOptions();
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}