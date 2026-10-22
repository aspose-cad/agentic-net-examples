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
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cad = (CadImage)cadImage;

                // List to collect all text entities
                List<CadEntityBase> textEntities = new List<CadEntityBase>();

                // Inspect top‑level drawing entities
                foreach (CadEntityBase entity in cad.Entities)
                {
                    // Filter for text entities
                    if (entity is CadText)
                    {
                        textEntities.Add(entity);
                    }
                }

                // Also inspect entities inside block definitions
                foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity is CadText)
                        {
                            textEntities.Add(entity);
                        }
                    }
                }

                // Output the result
                Console.WriteLine($"Found {textEntities.Count} text entities.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}