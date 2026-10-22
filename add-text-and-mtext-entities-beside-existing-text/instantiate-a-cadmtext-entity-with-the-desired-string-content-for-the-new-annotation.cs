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
            // Paths
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "output.dwg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Create a new MText entity
                CadMText mText = new CadMText
                {
                    Text = "Hello Aspose CAD",
                    InsertionPoint = new Cad3DPoint(10, 10, 0),
                    InitialTextHeight = 2.5
                };

                // Locate the Model Space block (commonly "*Model_Space")
                CadBlockEntity modelSpace = null;
                if (cadImage.BlockEntities.ContainsKey("*Model_Space"))
                {
                    modelSpace = cadImage.BlockEntities["*Model_Space"];
                }
                else
                {
                    // Fallback: use the first available block
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        modelSpace = block;
                        break;
                    }
                }

                // Add the MText entity to the block's entity collection
                if (modelSpace != null)
                {
                    modelSpace.AddEntity(mText);
                }

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