using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadInsertExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths to the target DXF, source DXF and the output DXF
                string targetPath = "target.dxf";
                string sourcePath = "source.dxf";
                string outputPath = "merged.dxf";

                // Verify input files exist
                if (!File.Exists(targetPath) || !File.Exists(sourcePath))
                {
                    Console.Error.WriteLine("One or both input DXF files were not found.");
                    return;
                }

                // Load the target and source drawings
                using (Image targetImage = Image.Load(targetPath))
                using (Image sourceImage = Image.Load(sourcePath))
                {
                    // Cast loaded images to CadImage to access CAD object model
                    CadImage targetCad = (CadImage)targetImage;
                    CadImage sourceCad = (CadImage)sourceImage;

                    // Insert each block from the source drawing into the target drawing
                    foreach (CadBlockEntity sourceBlock in sourceCad.BlockEntities.ValuesTyped)
                    {
                        // Ensure block name uniqueness in the target drawing
                        string blockName = sourceBlock.Name;
                        bool nameExists = targetCad.BlockEntities.ValuesTyped.Any(b => b.Name == blockName);
                        if (nameExists)
                        {
                            blockName = blockName + "_src";
                        }

                        // Rename the block (modifies the source block, which is acceptable for this example)
                        sourceBlock.Name = blockName;

                        // Add the block definition to the target drawing
                        // BlockEntities collection expects a name and the block entity
                        targetCad.BlockEntities.Add(blockName, sourceBlock);

                        // Create a block reference (insert entity) in the target drawing
                        CadInsertObject insertEntity = new CadInsertObject
                        {
                            Name = blockName,
                            InsertionPoint = new Cad3DPoint(0, 0, 0) // Insert at origin; adjust as needed
                        };

                        // Append the insert entity to the target drawing's entity collection
                        targetCad.Entities = targetCad.Entities
                            .Concat(new CadEntityBase[] { insertEntity })
                            .ToArray();
                    }

                    // Save the merged drawing as DXF
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    targetCad.Save(outputPath, dxfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}