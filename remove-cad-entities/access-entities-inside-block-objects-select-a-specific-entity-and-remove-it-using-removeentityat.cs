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
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' not found.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Locate the block named "MyBlock"
                CadBlockEntity targetBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "MyBlock")
                    {
                        targetBlock = block;
                        break;
                    }
                }

                if (targetBlock != null)
                {
                    // Find the first CadLine entity inside the block
                    CadEntityBase entityToRemove = null;
                    foreach (CadEntityBase entity in targetBlock.Entities)
                    {
                        if (entity is CadLine)
                        {
                            entityToRemove = entity;
                            break;
                        }
                    }

                    if (entityToRemove != null)
                    {
                        // Remove the identified entity
                        if (targetBlock.Entities.Remove(entityToRemove))
                        {
                            Console.WriteLine($"Removed a CadLine entity from block '{targetBlock.Name}'.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to remove the entity.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No CadLine entity found in the block.");
                    }
                }
                else
                {
                    Console.WriteLine("Block 'MyBlock' not found.");
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}