using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg"; // placeholder path

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Collect external block references (Xrefs) from block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block definition: {block.Name}");

                    foreach (CadEntityBase innerEntity in block.Entities)
                    {
                        if (innerEntity is CadInsertObject insertObj)
                        {
                            Console.WriteLine($"  Insert reference inside block: {insertObj.Name} (Original: {insertObj.OriginalBlockName})");
                        }
                    }
                }

                // Collect external block references used directly in the drawing
                foreach (CadEntityBase topEntity in cadImage.Entities)
                {
                    if (topEntity is CadInsertObject topInsert)
                    {
                        Console.WriteLine($"Top-level insert reference: {topInsert.Name} (Original: {topInsert.OriginalBlockName})");
                    }
                }

                // Further block insertion operations can be performed here.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}