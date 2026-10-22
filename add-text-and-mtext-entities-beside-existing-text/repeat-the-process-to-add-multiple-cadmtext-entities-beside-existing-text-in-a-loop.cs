using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Locate the Model Space block where drawing entities are stored
                CadBlockEntity modelSpace = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "*Model_Space")
                    {
                        modelSpace = block;
                        break;
                    }
                }

                if (modelSpace == null)
                {
                    Console.WriteLine("Model space block not found.");
                    return;
                }

                // Collect original MText entities to avoid modifying the collection while iterating
                var originalMTexts = new List<CadMText>();
                foreach (CadEntityBase entity in modelSpace.Entities)
                {
                    if (entity is CadMText mtext)
                    {
                        originalMTexts.Add(mtext);
                    }
                }

                // Add new MText entities beside each existing one
                foreach (CadMText originalMText in originalMTexts)
                {
                    // Create a new MText entity
                    CadMText newMText = new CadMText
                    {
                        // Copy the text content
                        Text = originalMText.Text,
                        // Preserve the original text height
                        InitialTextHeight = originalMText.InitialTextHeight
                    };

                    // Position the new text 10 units to the right of the original
                    Cad3DPoint originalPoint = originalMText.InsertionPoint;
                    newMText.InsertionPoint = new Cad3DPoint(originalPoint.X + 10.0, originalPoint.Y, originalPoint.Z);

                    // Add the new MText to the Model Space block
                    modelSpace.AddEntity(newMText);
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