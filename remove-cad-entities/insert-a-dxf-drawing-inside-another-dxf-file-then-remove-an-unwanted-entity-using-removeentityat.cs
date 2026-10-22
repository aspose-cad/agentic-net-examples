using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for source (the drawing to insert) and target (the drawing that will receive the block)
            string sourcePath = "source.dxf";
            string targetPath = "target.dxf";
            string outputPath = "result.dxf";

            // Guard against missing files
            if (!File.Exists(sourcePath) || !File.Exists(targetPath))
            {
                Console.WriteLine("Source or target file does not exist.");
                return;
            }

            // Load the CAD images
            CadImage sourceCad = (CadImage)Image.Load(sourcePath);
            CadImage targetCad = (CadImage)Image.Load(targetPath);

            // -------------------------------------------------------------
            // 1. Insert a block from the source drawing into the target drawing
            // -------------------------------------------------------------
            CadBlockEntity sourceBlock = sourceCad.BlockEntities.Values
                .Cast<CadBlockEntity>()
                .FirstOrDefault();

            if (sourceBlock != null)
            {
                string blockName = sourceBlock.Name;

                // Add the block to the target drawing if it's not already present
                if (!targetCad.BlockEntities.ContainsKey(blockName))
                {
                    targetCad.BlockEntities.Add(blockName, sourceBlock);
                }

                // Create an insert object that references the block
                CadInsertObject insert = new CadInsertObject
                {
                    Name = blockName,
                    InsertionPoint = new Cad3DPoint(0, 0, 0),
                    ScaleX = 1.0,
                    ScaleY = 1.0,
                    ScaleZ = 1.0,
                    RotationAngle = 0.0
                };

                // Append the insert object to the target drawing's entities
                targetCad.Entities = targetCad.Entities
                    .Concat(new CadEntityBase[] { insert })
                    .ToArray();
            }

            // -------------------------------------------------------------
            // 2. Remove an unwanted entity from the target drawing
            // -------------------------------------------------------------
            int unwantedIndex = 0;
            List<CadEntityBase> entityList = targetCad.Entities.ToList();

            if (unwantedIndex >= 0 && unwantedIndex < entityList.Count)
            {
                entityList.RemoveAt(unwantedIndex);
                targetCad.Entities = entityList.ToArray();
            }

            // -------------------------------------------------------------
            // 3. Save the modified drawing
            // -------------------------------------------------------------
            var saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert
            };

            targetCad.Save(outputPath);

            Console.WriteLine($"Drawing processed and saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}