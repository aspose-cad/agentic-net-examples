using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";
        string targetBlockName = "MyBlock";

        // Validate input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Locate the target block
                CadBlockEntity targetBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (string.Equals(block.Name, targetBlockName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetBlock = block;
                        break;
                    }
                }

                if (targetBlock == null)
                {
                    Console.WriteLine($"Block \"{targetBlockName}\" not found.");
                    return;
                }

                // Replace top‑level entities with those from the target block
                cadImage.Entities = targetBlock.Entities.ToArray();

                // Remove all other block definitions
                List<CadBlockEntity> blocksToRemove = new List<CadBlockEntity>();
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (!string.Equals(block.Name, targetBlockName, StringComparison.OrdinalIgnoreCase))
                    {
                        blocksToRemove.Add(block);
                    }
                }

                foreach (CadBlockEntity block in blocksToRemove)
                {
                    cadImage.BlockEntities.Remove(block.Name);
                }

                // Save cleaned drawing as PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Cleaned drawing saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}