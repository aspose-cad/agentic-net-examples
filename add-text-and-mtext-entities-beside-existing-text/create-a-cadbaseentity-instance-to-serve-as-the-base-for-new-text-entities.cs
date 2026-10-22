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
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Get a SoftOwner value from the first layout (required for new entities)
                string softOwner = cadImage.Layouts.Values.FirstOrDefault()?.BlockTableRecordHandle ?? string.Empty;

                // Work on a mutable list of entities
                List<CadEntityBase> entities = cadImage.Entities.ToList();

                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText existingText)
                    {
                        double offsetX = 10.0;
                        double newX = existingText.FirstAlignment.X + offsetX;
                        double newY = existingText.FirstAlignment.Y;
                        double height = existingText.TextHeight;

                        CadText newText = new CadText
                        {
                            DefaultValue = "New Text",
                            FirstAlignment = new Cad3DPoint(newX, newY, 0),
                            TextHeight = height,
                            TextRotation = existingText.TextRotation,
                            LayerName = existingText.LayerName,
                            SoftOwner = softOwner
                        };

                        entities.Add(newText);
                    }
                    else if (entity is CadMText existingMText)
                    {
                        double offsetX = 10.0;
                        double newX = existingMText.InsertionPoint.X + offsetX;
                        double newY = existingMText.InsertionPoint.Y;
                        double height = existingMText.InitialTextHeight;

                        CadMText newMText = new CadMText
                        {
                            Text = "New MText",
                            InsertionPoint = new Cad3DPoint(newX, newY, 0),
                            InitialTextHeight = height,
                            RotationAngleRad = existingMText.RotationAngleRad,
                            LayerName = existingMText.LayerName,
                            SoftOwner = softOwner
                        };

                        entities.Add(newMText);
                    }
                }

                cadImage.Entities = entities.ToArray();

                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}