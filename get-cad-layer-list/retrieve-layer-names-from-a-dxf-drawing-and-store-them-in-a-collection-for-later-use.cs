using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder path to the DXF file
        string inputPath = "sample.dxf";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Retrieve layer names from the drawing
                HashSet<string> layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Collect layer names from top-level entities
                foreach (var entity in cad.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        layerNames.Add(entity.LayerName);
                    }
                }

                // Also collect layer names from block entities (if any)
                foreach (var block in cad.BlockEntities.ValuesTyped)
                {
                    foreach (var blockEntity in block.Entities)
                    {
                        if (!string.IsNullOrEmpty(blockEntity.LayerName))
                        {
                            layerNames.Add(blockEntity.LayerName);
                        }
                    }
                }

                // Output the collected layer names
                Console.WriteLine("Layers found in the drawing:");
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