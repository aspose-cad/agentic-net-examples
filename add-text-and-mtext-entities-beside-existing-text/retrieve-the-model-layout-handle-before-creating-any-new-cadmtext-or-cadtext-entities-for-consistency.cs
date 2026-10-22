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

                // Retrieve the Model layout handle
                var modelLayout = cadImage.Layouts.Values.FirstOrDefault(l => l.LayoutName == "Model");
                string modelHandle = modelLayout?.BlockTableRecordHandle ?? string.Empty;

                // Create a CadText entity
                var text = new CadText
                {
                    DefaultValue = "Added Text",
                    FirstAlignment = new Cad3DPoint(10.0, 10.0, 0.0),
                    TextHeight = 2.5,
                    TextRotation = 0.0,
                    SoftOwner = modelHandle
                };

                // Create a CadMText entity
                var mtext = new CadMText
                {
                    Text = "Added MText",
                    InsertionPoint = new Cad3DPoint(20.0, 20.0, 0.0),
                    InitialTextHeight = 2.5,
                    RotationAngleRad = 0.0,
                    SoftOwner = modelHandle
                };

                // Add the new entities to the model space
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { text, mtext })
                    .ToArray();

                // Save the modified CAD file
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}