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
            Console.Error.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Load an existing DWG drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Find the model layout (usually named "Model")
                var modelLayout = cadImage.Layouts.Values.FirstOrDefault(l =>
                    string.Equals(l.LayoutName, "Model", StringComparison.OrdinalIgnoreCase));

                if (modelLayout == null)
                {
                    Console.Error.WriteLine("Model layout not found in the CAD file.");
                    return;
                }

                // Get the block that corresponds to model space
                var modelBlock = cadImage.BlockEntities.ValuesTyped.FirstOrDefault();
                if (modelBlock == null)
                {
                    Console.Error.WriteLine("Model space block not found.");
                    return;
                }

                // Create a new TEXT entity
                CadText textEntity = new CadText
                {
                    DefaultValue = "Hello Aspose CAD",
                    FirstAlignment = new Cad3DPoint(10.0, 20.0, 0.0),
                    TextHeight = 2.5,
                    TextRotation = 0.0,
                    SoftOwner = modelLayout.BlockTableRecordHandle
                };

                // Add the TEXT entity to model space
                modelBlock.AddEntity(textEntity);

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified drawing back to DWG format
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}