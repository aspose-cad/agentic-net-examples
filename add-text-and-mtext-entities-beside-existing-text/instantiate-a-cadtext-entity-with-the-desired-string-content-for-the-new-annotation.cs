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
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                if (!cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpace))
                {
                    Console.WriteLine("Model space block not found.");
                    return;
                }

                CadText referenceText = null;
                foreach (CadEntityBase entity in modelSpace.Entities)
                {
                    if (entity is CadText txt)
                    {
                        referenceText = txt;
                        break;
                    }
                }

                if (referenceText == null)
                {
                    Console.WriteLine("No CadText entity found in model space.");
                    return;
                }

                // Create new CadText
                Cad3DPoint newTextLocation = new Cad3DPoint(
                    referenceText.FirstAlignment.X + 10,
                    referenceText.FirstAlignment.Y,
                    referenceText.FirstAlignment.Z);

                CadText newText = new CadText
                {
                    DefaultValue = "New Text",
                    FirstAlignment = newTextLocation,
                    TextHeight = referenceText.TextHeight,
                    TextRotation = referenceText.TextRotation,
                    SoftOwner = string.Empty
                };

                // Create new CadMText
                Cad3DPoint newMTextLocation = new Cad3DPoint(
                    referenceText.FirstAlignment.X,
                    referenceText.FirstAlignment.Y - 10,
                    referenceText.FirstAlignment.Z);

                CadMText newMText = new CadMText
                {
                    Text = "New MText",
                    InsertionPoint = newMTextLocation,
                    InitialTextHeight = referenceText.TextHeight,
                    RotationAngleRad = referenceText.TextRotation,
                    SoftOwner = string.Empty
                };

                // Add entities to model space
                modelSpace.AddEntity(newText);
                modelSpace.AddEntity(newMText);

                // Save the modified CAD file
                cadImage.Save(outputPath);
            }

            Console.WriteLine("New TEXT and MTEXT entities added successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}