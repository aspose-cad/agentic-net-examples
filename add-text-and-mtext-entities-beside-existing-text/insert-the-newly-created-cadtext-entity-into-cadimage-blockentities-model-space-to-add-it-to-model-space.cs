using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AddTextAndMTextEntities
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWG/DXF file
                string inputPath = "input.dwg";

                // Verify that the input file exists before attempting to load it
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file \"{inputPath}\" does not exist.");
                    return;
                }

                // Load the CAD file and cast to CadImage to access CAD object model
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Retrieve the model space block where drawing entities are stored
                    if (!cadImage.BlockEntities.ContainsKey("*MODEL_SPACE"))
                    {
                        Console.WriteLine("Model space block \"*MODEL_SPACE\" not found.");
                        return;
                    }

                    CadBlockEntity modelSpaceBlock = cadImage.BlockEntities["*MODEL_SPACE"];

                    // Create a CadText entity
                    CadText cadText = new CadText
                    {
                        // Text content
                        DefaultValue = "Sample Text",
                        // Insertion point (X, Y, Z)
                        FirstAlignment = new Cad3DPoint(0, 0, 0),
                        // Optional height (default 2.5 if not set)
                        TextHeight = 2.5
                    };

                    // Create a CadMText entity
                    CadMText cadMText = new CadMText
                    {
                        // Text content for MText
                        Text = "Multi-line\nSample Text",
                        // Insertion point (X, Y, Z)
                        InsertionPoint = new Cad3DPoint(10, 10, 0),
                        // Optional height
                        InitialTextHeight = 2.5
                    };

                    // Add the new entities to the model space block
                    modelSpaceBlock.AddEntity(cadText);
                    modelSpaceBlock.AddEntity(cadMText);

                    // Save the modified drawing to a new file
                    string outputPath = "output.dwg";
                    cadImage.Save(outputPath);
                    Console.WriteLine($"Modified drawing saved to \"{outputPath}\".");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}