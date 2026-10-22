using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class CadMergeExample
{
    static void Main()
    {
        // Paths for the target drawing, source drawing and the output file
        string targetPath = @"C:\CAD\target.dxf";
        string sourcePath = @"C:\CAD\source.dxf";
        string outputPath = @"C:\CAD\output\combined.dxf";

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the target and source CAD files
            using (var targetImage = (CadImage)Image.Load(targetPath))
            using (var sourceImage = (CadImage)Image.Load(sourcePath))
            {
                // Get the first block from the source drawing
                var sourceBlock = sourceImage.BlockEntities.ValuesTyped.FirstOrDefault();
                if (sourceBlock == null)
                {
                    throw new InvalidOperationException("Source drawing does not contain any block definitions.");
                }

                // Remove the first nested entity inside the source block (example of removal)
                var blockEntities = sourceBlock.Entities.ToList();
                if (blockEntities.Any())
                {
                    // Remove the first entity
                    blockEntities.RemoveAt(0);
                    // Clear existing entities and re‑add the filtered collection
                    sourceBlock.Entities.Clear();
                    foreach (var ent in blockEntities)
                    {
                        sourceBlock.AddEntity(ent);
                    }
                }

                // Create an insert object that references the (now modified) source block
                var insert = new CadInsertObject
                {
                    // The block name to insert – use the original block name
                    Name = sourceBlock.Name,
                    // Position where the block will be placed in the target drawing
                    InsertionPoint = new Cad3DPoint(0, 0, 0),
                    // Default scaling and rotation
                    ScaleX = 1.0f,
                    ScaleY = 1.0f,
                    ScaleZ = 1.0f,
                    RotationAngle = 0.0f
                };

                // Append the insert object to the target drawing's entities collection
                // CadImage.Entities is IEnumerable<CadEntityBase>, so we create a new array that includes the insert
                targetImage.Entities = targetImage.Entities
                    .Concat(new CadEntityBase[] { insert })
                    .ToArray();

                // Save the combined drawing as DXF
                var dxfOptions = new DxfOptions
                {
                    // Preserve the original CAD objects (no rasterization)
                    OutputMode = CadOutputMode.Convert
                };

                targetImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine("Combined CAD file saved successfully to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during CAD processing: " + ex.Message);
        }
    }
}