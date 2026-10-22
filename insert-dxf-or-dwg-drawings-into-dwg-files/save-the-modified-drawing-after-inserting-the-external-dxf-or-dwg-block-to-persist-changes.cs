using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class InsertExternalBlock
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string sourcePath = @"C:\CAD\source.dwg";
        string blockPath = @"C:\CAD\external_block.dxf";
        string outputPath = @"C:\CAD\output.dwg";

        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }
        if (!File.Exists(blockPath))
        {
            Console.WriteLine($"Block file not found: {blockPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
            using (CadImage blockImage = (CadImage)Image.Load(blockPath))
            {
                // Retrieve the first block name from the external file
                string externalBlockName = blockImage.BlockEntities.Keys
                    .Cast<string>()
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(externalBlockName))
                {
                    Console.WriteLine("No block found in the external file.");
                    return;
                }

                // Create an insert object for the external block
                CadInsertObject insert = new CadInsertObject
                {
                    Name = externalBlockName,
                    ScaleX = 1.0f,
                    ScaleY = 1.0f,
                    ScaleZ = 1.0f,
                    RotationAngle = 0.0f
                };

                const string modelSpaceKey = "*Model_Space";
                if (!cadImage.BlockEntities.ContainsKey(modelSpaceKey))
                {
                    Console.WriteLine("Model space block not found in the source drawing.");
                    return;
                }

                CadBlockEntity modelSpace = cadImage.BlockEntities[modelSpaceKey];
                modelSpace.AddEntity(insert);

                // Save the modified drawing
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD files: {ex.Message}");
        }
    }
}