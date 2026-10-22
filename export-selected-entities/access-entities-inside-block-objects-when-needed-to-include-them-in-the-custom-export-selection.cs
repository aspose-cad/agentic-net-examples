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

                // List block definitions and their inner entities
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        Console.WriteLine($"  Entity Type: {entity.GetType().Name}");
                    }
                }

                // Process top‑level entities and block inserts
                foreach (CadEntityBase topEntity in cadImage.Entities)
                {
                    if (topEntity is CadInsertObject insert)
                    {
                        Console.WriteLine($"Insert of block: {insert.Name}");

                        // Find the corresponding block definition without LINQ
                        CadBlockEntity referencedBlock = null;
                        foreach (CadBlockEntity b in cadImage.BlockEntities.ValuesTyped)
                        {
                            if (b.Name == insert.Name)
                            {
                                referencedBlock = b;
                                break;
                            }
                        }

                        if (referencedBlock != null)
                        {
                            foreach (CadEntityBase innerEntity in referencedBlock.Entities)
                            {
                                Console.WriteLine($"  Inner Entity Type: {innerEntity.GetType().Name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("  Block definition not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Top‑level Entity Type: {topEntity.GetType().Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}