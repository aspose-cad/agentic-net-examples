using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the DRC drawing file
            string filePath = "sample.drc";

            // Verify the input file exists
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine($"Input file not found: {filePath}");
                return;
            }

            // Load the drawing
            using (Image image = Image.Load(filePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Dictionary to hold entity counts per layer
                Dictionary<string, int> layerEntityCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                // Iterate over top-level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Get layer name; if null or empty, use default layer name
                    string layerName = entity.LayerName;
                    if (string.IsNullOrEmpty(layerName))
                    {
                        layerName = "0"; // default layer name in many CAD files
                    }

                    // Increment count for the layer
                    if (layerEntityCounts.ContainsKey(layerName))
                    {
                        layerEntityCounts[layerName] += 1;
                    }
                    else
                    {
                        layerEntityCounts[layerName] = 1;
                    }
                }

                // Output the counts
                Console.WriteLine("Entity count per layer:");
                foreach (KeyValuePair<string, int> kvp in layerEntityCounts)
                {
                    Console.WriteLine($"Layer \"{kvp.Key}\": {kvp.Value} entities");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}