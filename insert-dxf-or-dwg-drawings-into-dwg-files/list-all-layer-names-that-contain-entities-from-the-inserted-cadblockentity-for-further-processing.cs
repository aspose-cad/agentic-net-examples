using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayerLister
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file (DWG/DXF). Provide via command line or use default.
                string cadFilePath = args.Length > 0 ? args[0] : "sample.dwg";

                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD file.
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast to CadImage to access CAD-specific members.
                    CadImage cadImage = (CadImage)image;

                    // Use a set to avoid duplicate layer names.
                    HashSet<string> layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    // Iterate over all block definitions in the drawing.
                    foreach (CadBlockEntity blockEntity in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Iterate over entities contained in each block.
                        foreach (CadEntityBase entity in blockEntity.Entities)
                        {
                            // Collect the layer name if it is defined.
                            if (!string.IsNullOrEmpty(entity.LayerName))
                            {
                                layerNames.Add(entity.LayerName);
                            }
                        }
                    }

                    // Output the collected layer names.
                    Console.WriteLine("Layers containing entities from inserted block definitions:");
                    foreach (string layerName in layerNames)
                    {
                        Console.WriteLine(layerName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}