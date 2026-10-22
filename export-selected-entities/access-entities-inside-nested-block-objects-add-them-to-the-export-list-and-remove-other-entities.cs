using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List to collect entities from nested blocks
                List<CadEntityBase> exportEntities = new List<CadEntityBase>();

                // Iterate over all block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    CollectBlockEntities(block, cadImage, exportEntities);
                }

                // Replace the drawing's top‑level entities with the collected ones
                cadImage.Entities = exportEntities.ToArray();

                // Save the modified drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Recursively collect entities from a block and any nested block inserts
    private static void CollectBlockEntities(CadBlockEntity block, CadImage cadImage, List<CadEntityBase> list)
    {
        foreach (CadEntityBase entity in block.Entities)
        {
            if (entity is CadInsertObject insert)
            {
                // Resolve the referenced block by its name
                CadBlockEntity referencedBlock = FindBlockByName(cadImage, insert.Name);
                if (referencedBlock != null)
                {
                    // Recursively collect entities from the referenced block
                    CollectBlockEntities(referencedBlock, cadImage, list);
                }
            }
            else
            {
                // Add non‑insert entities directly
                list.Add(entity);
            }
        }
    }

    // Helper method to locate a block definition by name
    private static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
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