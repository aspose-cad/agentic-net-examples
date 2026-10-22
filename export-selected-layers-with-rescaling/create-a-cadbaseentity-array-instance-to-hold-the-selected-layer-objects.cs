using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the CAD file
                string inputPath = "sample.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Name of the layer to select
                string targetLayer = "0";

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Collect entities that belong to the target layer
                    List<CadEntityBase> layerEntities = new List<CadEntityBase>();
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (string.Equals(entity.LayerName, targetLayer, StringComparison.OrdinalIgnoreCase))
                        {
                            layerEntities.Add(entity);
                        }
                    }

                    // Create an array to hold the selected layer objects
                    CadEntityBase[] selectedLayerObjects = layerEntities.ToArray();

                    // Example output
                    Console.WriteLine($"Found {selectedLayerObjects.Length} entities on layer '{targetLayer}'.");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}