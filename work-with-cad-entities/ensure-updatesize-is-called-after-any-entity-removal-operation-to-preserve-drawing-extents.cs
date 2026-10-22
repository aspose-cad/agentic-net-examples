using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

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

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the DWG/DXF drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Remove all line entities that belong to layer "0" from the top‑level drawing entities
                var filteredTopEntities = cadImage.Entities
                    .Where(e => !(e.LayerName == "0" && e is CadLine))
                    .ToArray();

                // Assign the filtered collection back to the image
                cadImage.Entities = filteredTopEntities;

                // Find a block definition named "BLOCK1"
                CadBlockEntity targetBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "BLOCK1")
                    {
                        targetBlock = block;
                        break;
                    }
                }

                // If the block exists, remove all line entities inside that block
                if (targetBlock != null)
                {
                    var blockEntities = targetBlock.Entities
                        .Where(e => !(e is CadLine))
                        .ToList();

                    // Clear existing entities and add the filtered ones back
                    targetBlock.Entities.Clear();
                    foreach (var ent in blockEntities)
                    {
                        targetBlock.AddEntity(ent);
                    }
                }

                // Recalculate drawing extents after entity removal
                cadImage.UpdateSize();

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