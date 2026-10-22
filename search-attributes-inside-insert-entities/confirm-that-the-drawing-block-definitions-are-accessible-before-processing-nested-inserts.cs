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
        const string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                // Verify that block definitions are accessible
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block definition: {block.Name}, Entity count: {block.Entities.Count}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        Console.WriteLine($"  Entity type: {entity.TypeName}");
                    }
                }

                // Process top‑level entities, handling nested block inserts
                foreach (CadEntityBase topEntity in cadImage.Entities)
                {
                    if (topEntity is CadInsertObject insert)
                    {
                        // Ensure the referenced block definition exists
                        CadBlockEntity referencedBlock = FindBlockByName(cadImage, insert.Name);
                        if (referencedBlock != null)
                        {
                            Console.WriteLine($"Insert of block '{insert.Name}' with scale ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                            // Optionally inspect entities inside the inserted block
                            foreach (CadEntityBase innerEntity in referencedBlock.Entities)
                            {
                                Console.WriteLine($"    Inner entity type: {innerEntity.TypeName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Block definition for '{insert.Name}' not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Top‑level entity type: {topEntity.TypeName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
    {
        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
        {
            if (block.Name.Equals(blockName, StringComparison.OrdinalIgnoreCase))
            {
                return block;
            }
        }
        return null;
    }
}