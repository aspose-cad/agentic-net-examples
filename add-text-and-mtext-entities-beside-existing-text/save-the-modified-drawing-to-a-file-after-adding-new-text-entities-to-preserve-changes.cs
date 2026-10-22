using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Retrieve the Model Space block where drawing entities are stored
                if (!cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpace))
                {
                    Console.WriteLine("Model space block not found.");
                    return;
                }

                // Create a new CadText entity
                Cad3DPoint textLocation = new Cad3DPoint(10.0, 10.0, 0.0);
                CadText newText = new CadText
                {
                    DefaultValue = "New Text",
                    FirstAlignment = textLocation,
                    TextHeight = 2.5,
                    TextRotation = 0.0
                };

                // Create a new CadMText entity
                Cad3DPoint mtextLocation = new Cad3DPoint(20.0, 10.0, 0.0);
                CadMText newMText = new CadMText
                {
                    Text = "New MText",
                    InsertionPoint = mtextLocation,
                    InitialTextHeight = 2.5,
                    RotationAngleRad = 0.0
                };

                // Add the new entities to the Model Space block
                modelSpace.AddEntity(newText);
                modelSpace.AddEntity(newMText);

                // Ensure the output directory exists
                string outDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                // Save the modified drawing
                DwgOptions saveOptions = new DwgOptions();
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Drawing saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}