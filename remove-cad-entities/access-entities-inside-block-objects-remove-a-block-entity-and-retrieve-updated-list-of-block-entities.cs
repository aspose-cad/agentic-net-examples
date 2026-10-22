using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' not found.");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Locate the block definition named "MyBlock"
                CadBlockEntity targetBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "MyBlock")
                    {
                        targetBlock = block;
                        break;
                    }
                }

                if (targetBlock == null)
                {
                    Console.WriteLine("Block 'MyBlock' not found.");
                    return;
                }

                // List entities inside the block before removal
                Console.WriteLine("Entities in block before removal:");
                foreach (CadEntityBase entity in targetBlock.Entities)
                {
                    Console.WriteLine("- " + entity.TypeName);
                }

                // Remove the first entity from the block (if any)
                if (targetBlock.Entities.Count > 0)
                {
                    CadEntityBase removedEntity = targetBlock.Entities.ElementAt(0);
                    targetBlock.Entities.Remove(removedEntity);
                    Console.WriteLine($"Removed entity of type {removedEntity.TypeName}");
                }

                // Retrieve and list the updated block entities
                Console.WriteLine("Entities in block after removal:");
                foreach (CadEntityBase entity in targetBlock.Entities)
                {
                    Console.WriteLine("- " + entity.TypeName);
                }

                // Save the modified drawing to PDF (optional)
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Modified drawing saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}