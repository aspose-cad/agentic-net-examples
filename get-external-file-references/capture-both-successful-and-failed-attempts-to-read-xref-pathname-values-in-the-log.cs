using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        // Ensure the input file exists; create placeholder if missing
        if (!File.Exists(inputPath))
        {
            // Create an empty placeholder file
            File.WriteAllBytes(inputPath, new byte[0]);
            Console.WriteLine($"Placeholder CAD file created at '{inputPath}'.");
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = (CadImage)cadImage;

                // Top‑level entities
                foreach (CadEntityBase entity in cad.Entities)
                {
                    if (entity is CadInsertObject insert)
                    {
                        ProcessInsert(insert, $"Insert '{insert.Name}'");
                    }
                }

                // Block definitions
                foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        if (blockEntity is CadInsertObject blockInsert)
                        {
                            ProcessInsert(blockInsert, $"Block '{block.Name}' Insert '{blockInsert.Name}'");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file '{inputPath}': {ex.Message}");
        }
    }

    private static void ProcessInsert(CadInsertObject insert, string context)
    {
        try
        {
            // Attempt to read a non‑existent PathName property via reflection
            var prop = insert.GetType().GetProperty("PathName");
            if (prop != null)
            {
                var value = prop.GetValue(insert) as string;
                Console.WriteLine($"XRef PathName ({context}): {value}");
            }
            else
            {
                throw new MissingMemberException("PathName property not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read XRef PathName for {context}: {ex.Message}");
        }
    }
}