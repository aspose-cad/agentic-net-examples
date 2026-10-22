using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityFilter
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the CAD file
                string cadFilePath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {cadFilePath}");
                    return;
                }

                // Line weight threshold for filtering
                int lineWeightThreshold = 20;

                // Load the CAD image and cast to CadImage
                using (Image image = Image.Load(cadFilePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // List to hold filtered entities.
                    // Note: The SDK provides CadEntityBase as the base type for entities.
                    // There is no CadEntityBase class; therefore we collect CadEntityBase objects.
                    List<CadEntityBase> filteredEntities = new List<CadEntityBase>();

                    // Iterate over top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Filter by line weight
                        if (entity.LineWeight > lineWeightThreshold)
                        {
                            filteredEntities.Add(entity);
                        }
                    }

                    // Output the result
                    Console.WriteLine($"Found {filteredEntities.Count} entities with line weight greater than {lineWeightThreshold}.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}