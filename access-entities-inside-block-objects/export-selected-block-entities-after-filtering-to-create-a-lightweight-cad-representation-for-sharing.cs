using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Path for the lightweight CAD output
            string outputPath = "output/lightweight.dwg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Collect names of blocks that do NOT meet the filter criteria
                List<string> blockNamesToRemove = new List<string>();

                // Iterate using ValuesTyped as required
                foreach (CadBlockEntity blockEntity in cadImage.BlockEntities.ValuesTyped)
                {
                    // Assuming CadBlockEntity has a Name property
                    string blockName = blockEntity.Name;

                    // Example filter: keep only blocks whose name starts with "A"
                    if (!blockName.StartsWith("A", StringComparison.OrdinalIgnoreCase))
                    {
                        blockNamesToRemove.Add(blockName);
                    }
                }

                // Remove the unwanted blocks from the image
                foreach (string name in blockNamesToRemove)
                {
                    cadImage.BlockEntities.Remove(name);
                }

                // Save the filtered CAD image
                cadImage.Save(outputPath, null);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}