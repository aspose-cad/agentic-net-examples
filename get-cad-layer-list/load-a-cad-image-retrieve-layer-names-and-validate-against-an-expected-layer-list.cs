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
        // Path to the CAD file
        string inputPath = "input.dwg";

        // Expected layer names
        string[] expectedLayers = new string[] { "Layer1", "Layer2", "Layer3" };

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD image
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Retrieve actual layer names from entities
                HashSet<string> actualLayerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        actualLayerNames.Add(entity.LayerName);
                    }
                }

                // Also include entities inside block definitions
                foreach (var blockEntry in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase blockEntity in blockEntry.Entities)
                    {
                        if (!string.IsNullOrEmpty(blockEntity.LayerName))
                        {
                            actualLayerNames.Add(blockEntity.LayerName);
                        }
                    }
                }

                // Validate layers against expected list
                bool allMatch = true;

                // Check for missing expected layers
                foreach (string expected in expectedLayers)
                {
                    if (!actualLayerNames.Contains(expected))
                    {
                        Console.WriteLine($"Missing expected layer: {expected}");
                        allMatch = false;
                    }
                }

                // Check for unexpected layers
                foreach (string actual in actualLayerNames)
                {
                    if (Array.IndexOf(expectedLayers, actual) < 0)
                    {
                        Console.WriteLine($"Unexpected layer found: {actual}");
                        allMatch = false;
                    }
                }

                if (allMatch)
                {
                    Console.WriteLine("All expected layers are present and no unexpected layers were found.");
                }
                else
                {
                    Console.WriteLine("Layer validation completed with mismatches.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}