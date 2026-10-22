using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                // Find an existing CadText entity to use as reference
                CadText existingText = cadImage.Entities.OfType<CadText>().FirstOrDefault();

                if (existingText == null)
                {
                    Console.WriteLine("No CadText entity found in the drawing.");
                    return;
                }

                // Create a new CadText entity beside the existing one
                CadText newText = new CadText
                {
                    DefaultValue = "Added Text",
                    FirstAlignment = new Cad3DPoint(
                        existingText.FirstAlignment.X + 10,
                        existingText.FirstAlignment.Y,
                        existingText.FirstAlignment.Z),
                    LayerName = existingText.LayerName
                };

                // Append the new text to the drawing entities
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newText }).ToArray();

                // Obtain a layout's BlockTableRecordHandle for SoftOwner
                string blockTableHandle = cadImage.Layouts.Values
                    .Cast<CadLayout>()
                    .FirstOrDefault()?.BlockTableRecordHandle;

                // Locate a block to own a new CadMText entity
                CadBlockEntity targetBlock = cadImage.BlockEntities.Values
                    .Cast<CadBlockEntity>()
                    .FirstOrDefault();

                if (targetBlock != null && !string.IsNullOrEmpty(blockTableHandle))
                {
                    CadMText newMText = new CadMText
                    {
                        Text = "Added MText",
                        InsertionPoint = new Cad3DPoint(0, 0, 0),
                        SoftOwner = blockTableHandle
                    };

                    targetBlock.AddEntity(newMText);
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