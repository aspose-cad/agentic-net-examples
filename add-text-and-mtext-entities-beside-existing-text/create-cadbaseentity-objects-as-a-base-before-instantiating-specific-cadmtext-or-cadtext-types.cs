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
                CadImage cadImage = (CadImage)image;

                // Determine a SoftOwner (use the first layout's block table record handle if available)
                string softOwner = cadImage.Layouts.Values.FirstOrDefault()?.BlockTableRecordHandle ?? string.Empty;

                // Create base entity array
                CadEntityBase[] newEntities = new CadEntityBase[2];

                // Create a new single-line text entity
                CadText newText = new CadText
                {
                    DefaultValue = "New Text",
                    FirstAlignment = new Cad3DPoint(10.0, 10.0, 0.0),
                    TextHeight = 2.5,
                    TextRotation = 0.0,
                    LayerName = "0",
                    SoftOwner = softOwner
                };
                newEntities[0] = newText;

                // Create a new multi-line text (MText) entity
                CadMText newMText = new CadMText
                {
                    Text = "Multiline\nText",
                    InsertionPoint = new Cad3DPoint(10.0, 15.0, 0.0),
                    InitialTextHeight = 2.5,
                    RotationAngleRad = 0.0,
                    LayerName = "0",
                    SoftOwner = softOwner
                };
                newEntities[1] = newMText;

                // Append the new entities to the drawing
                cadImage.Entities = cadImage.Entities.Concat(newEntities).ToArray();

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}