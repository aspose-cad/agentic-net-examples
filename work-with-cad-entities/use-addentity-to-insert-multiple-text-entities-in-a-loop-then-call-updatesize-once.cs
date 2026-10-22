using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input and output paths
        string inputFilePath = @"C:\CAD\source.dxf";
        string outputFilePath = @"C:\CAD\result.dxf";

        // Verify input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFilePath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
            {
                // Select the first layout (e.g., Model space)
                CadLayout layout = cadImage.Layouts.Values.First();

                // Retrieve the block that corresponds to the layout
                // The layout's BlockTableRecordHandle points to a block in BlockEntities
                CadBlockEntity targetBlock = cadImage.BlockEntities[layout.BlockTableRecordHandle];

                // Insert multiple TEXT entities
                for (int i = 0; i < 5; i++)
                {
                    // Create a new CadText entity
                    CadText textEntity = new CadText
                    {
                        DefaultValue = $"Text {i + 1}",
                        FirstAlignment = new Cad3DPoint(10 * i, 20 * i, 0),
                        TextHeight = 2.5,
                        TextRotation = 0,
                        // Set SoftOwner to the block's handle (string)
                        SoftOwner = targetBlock.BlockHandle
                    };

                    // Add the entity to the block
                    targetBlock.AddEntity(textEntity);
                }

                // Update the drawing size after adding entities
                cadImage.UpdateSize();

                // Save the modified CAD drawing preserving editable objects
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert // keep objects editable
                };
                cadImage.Save(outputFilePath);
            }

            Console.WriteLine("CAD file processed and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}