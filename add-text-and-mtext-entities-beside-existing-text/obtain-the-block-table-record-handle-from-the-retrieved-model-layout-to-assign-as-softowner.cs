using System;
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
                var cadImage = (CadImage)image;

                // Get the first layout to obtain its BlockTableRecordHandle for SoftOwner
                var layout = cadImage.Layouts?.Values?.FirstOrDefault();
                string softOwner = layout?.BlockTableRecordHandle ?? string.Empty;

                var newEntities = cadImage.Entities.ToList();

                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText textEntity)
                    {
                        var newText = new CadText
                        {
                            FirstAlignment = new Cad3DPoint(
                                textEntity.FirstAlignment.X + 10,
                                textEntity.FirstAlignment.Y,
                                textEntity.FirstAlignment.Z),
                            TextHeight = textEntity.TextHeight,
                            TextRotation = textEntity.TextRotation,
                            DefaultValue = "Added Text",
                            SoftOwner = softOwner
                        };
                        newEntities.Add(newText);
                    }
                    else if (entity is CadMText mtextEntity)
                    {
                        var newMText = new CadMText
                        {
                            InsertionPoint = new Cad3DPoint(
                                mtextEntity.InsertionPoint.X + 10,
                                mtextEntity.InsertionPoint.Y,
                                mtextEntity.InsertionPoint.Z),
                            InitialTextHeight = mtextEntity.InitialTextHeight,
                            RotationAngleRad = mtextEntity.RotationAngleRad,
                            Text = "Added MText",
                            SoftOwner = softOwner
                        };
                        newEntities.Add(newMText);
                    }
                }

                cadImage.Entities = newEntities.ToArray();

                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}