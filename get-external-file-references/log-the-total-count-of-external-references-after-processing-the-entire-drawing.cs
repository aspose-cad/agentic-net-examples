using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                int externalReferenceCount = 0;

                // Iterate over top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Count each block insert (treated as an external reference for this example)
                    if (entity is CadInsertObject)
                    {
                        externalReferenceCount++;
                    }
                }

                // Iterate over block definitions and count inserts inside them
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        if (blockEntity is CadInsertObject)
                        {
                            externalReferenceCount++;
                        }
                    }
                }

                // Log the total count of external references
                Console.WriteLine($"Total external references: {externalReferenceCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}