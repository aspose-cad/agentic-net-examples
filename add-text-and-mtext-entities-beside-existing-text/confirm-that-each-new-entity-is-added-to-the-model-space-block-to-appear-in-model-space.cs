using System;
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
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Locate the "*MODEL_SPACE" block
                CadBlockEntity modelSpace = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "*MODEL_SPACE")
                    {
                        modelSpace = block;
                        break;
                    }
                }

                if (modelSpace == null)
                {
                    Console.WriteLine("MODEL_SPACE block not found.");
                    return;
                }

                // Create a new TEXT entity
                CadText newText = new CadText
                {
                    DefaultValue = "New Text",
                    FirstAlignment = new Cad3DPoint(10, 10, 0),
                    TextHeight = 2.5,
                    TextRotation = 0,
                    SoftOwner = modelSpace.SoftOwner
                };

                // Create a new MTEXT entity
                CadMText newMText = new CadMText
                {
                    Text = "New MText",
                    InsertionPoint = new Cad3DPoint(20, 20, 0),
                    InitialTextHeight = 2.5,
                    RotationAngleRad = 0,
                    SoftOwner = modelSpace.SoftOwner
                };

                // Add the new entities to the MODEL_SPACE block
                modelSpace.AddEntity(newText);
                modelSpace.AddEntity(newMText);

                // Save the modified drawing
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Text and MText entities added successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}