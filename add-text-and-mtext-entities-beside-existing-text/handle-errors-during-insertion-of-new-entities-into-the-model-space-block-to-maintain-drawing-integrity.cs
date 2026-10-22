using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddTextAndMTextEntities
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Locate the *MODEL_SPACE block
                CadBlockEntity modelSpaceBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name.Equals("*MODEL_SPACE", StringComparison.OrdinalIgnoreCase))
                    {
                        modelSpaceBlock = block;
                        break;
                    }
                }

                if (modelSpaceBlock == null)
                {
                    Console.WriteLine("MODEL_SPACE block not found.");
                    return;
                }

                try
                {
                    // Create a new TEXT entity
                    CadText newText = new CadText()
                    {
                        DefaultValue = "Added Text",
                        TextHeight = 2.5,
                        FirstAlignment = new Cad3DPoint(10, 10, 0)
                    };
                    modelSpaceBlock.AddEntity(newText);

                    // Create a new MTEXT entity
                    CadMText newMText = new CadMText()
                    {
                        Text = "Added MText",
                        InitialTextHeight = 2.5,
                        InsertionPoint = new Cad3DPoint(20, 20, 0)
                    };
                    modelSpaceBlock.AddEntity(newMText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while adding entities: {ex.Message}");
                    // Continue without rethrowing to preserve drawing integrity
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Drawing saved to {outputPath}");
            }
        }
    }
}