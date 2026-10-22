using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockSearchSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputPath = @"C:\CAD\sample.dxf";
            // Output CAD file path
            string outputPath = @"C:\CAD\output\modified.dxf";
            // Name of the block to locate
            string targetBlockName = "MyBlock";

            try
            {
                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Flag to indicate if the block was found
                    bool blockFound = false;

                    // Iterate through block entities using ValuesTyped as required
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Compare block name with the target name
                        if (string.Equals(block.Name, targetBlockName, StringComparison.OrdinalIgnoreCase))
                        {
                            blockFound = true;
                            Console.WriteLine($"Block \"{targetBlockName}\" found. Base point: {block.BasePoint}");

                            // Example: list entity types contained in the block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                Console.WriteLine($"  Entity type: {entity.TypeName}");
                            }

                            // If you need to modify the block, you can add entities here
                            // Example (commented out because it requires a concrete entity type):
                            // CadLine line = new CadLine(...);
                            // block.AddEntity(line);

                            break; // Block found, exit loop
                        }
                    }

                    if (!blockFound)
                    {
                        Console.WriteLine($"Block \"{targetBlockName}\" not found in the drawing.");
                    }

                    // Save the (potentially modified) drawing
                    DxfOptions saveOptions = new DxfOptions
                    {
                        // Preserve editable objects; convert mode can be used if needed
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath);
                    Console.WriteLine($"Drawing saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}