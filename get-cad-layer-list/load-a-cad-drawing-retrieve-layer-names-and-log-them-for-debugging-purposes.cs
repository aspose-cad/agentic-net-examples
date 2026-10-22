using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        // Placeholder input CAD file path
        string inputPath = "input.dxf";

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

                // Collect unique layer names
                HashSet<string> layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Top‑level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        layerNames.Add(entity.LayerName);
                    }
                }

                // Entities inside block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        if (!string.IsNullOrEmpty(blockEntity.LayerName))
                        {
                            layerNames.Add(blockEntity.LayerName);
                        }
                    }
                }

                // Log the layer names
                Console.WriteLine("Layer names in drawing:");
                foreach (string name in layerNames)
                {
                    Console.WriteLine(name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}