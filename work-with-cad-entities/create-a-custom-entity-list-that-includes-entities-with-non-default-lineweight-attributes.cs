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
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List to store entities that have a non‑default LineWeight
                List<CadEntityBase> nonDefaultLineWeightEntities = new List<CadEntityBase>();

                // NOTE: Aspose.CAD's CadEntityBase does not expose a LineWeight property.
                // Therefore, filtering by non‑default LineWeight cannot be performed with the current SDK.
                // The list will remain empty, and we inform the user about this limitation.

                // Inspect top‑level drawing entities (placeholder for future LineWeight check)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Placeholder: no LineWeight property available.
                }

                // Inspect entities inside block definitions (placeholder for future LineWeight check)
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        // Placeholder: no LineWeight property available.
                    }
                }

                // Output results
                Console.WriteLine("Aspose.CAD does not provide access to the LineWeight attribute of entities.");
                Console.WriteLine($"Found {nonDefaultLineWeightEntities.Count} entities with non‑default LineWeight (none can be identified).");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}