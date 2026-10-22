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

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Get the first layout (typically Model Space) to set SoftOwner
                var modelLayout = cadImage.Layouts.Values.FirstOrDefault();
                string ownerHandle = modelLayout?.BlockTableRecordHandle ?? string.Empty;

                // Create a new TEXT entity
                var textEntity = new CadText
                {
                    DefaultValue = "Hello, Aspose.CAD!",
                    FirstAlignment = new Cad3DPoint(10.0, 20.0, 0.0),
                    TextHeight = 2.5,
                    SoftOwner = ownerHandle
                };

                // Append the TEXT entity to model space entities
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { textEntity }).ToArray();

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