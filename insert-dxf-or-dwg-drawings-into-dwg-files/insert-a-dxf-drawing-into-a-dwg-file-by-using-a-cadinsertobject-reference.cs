using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace InsertDxfIntoDwg
{
    class Program
    {
        static void Main(string[] args)
        {
            const string targetPath = "TargetDrawing.dwg";
            const string sourcePath = "InsertDrawing.dxf";
            const string resultPath = "ResultWithInsertedDXF.dwg";

            if (!File.Exists(targetPath))
            {
                Console.WriteLine($"Target file not found: {targetPath}");
                return;
            }

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source DXF file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the target DWG file where the DXF will be inserted
                var targetCad = (CadImage)Image.Load(targetPath);

                // Load the source DXF file that will be inserted
                var sourceCad = (CadImage)Image.Load(sourcePath);

                // Create a new block definition that will hold the DXF entities
                var dxfBlock = new CadBlockEntity
                {
                    Name = "InsertedDXFBlock"
                };

                // Add all entities from the DXF file into the new block
                foreach (var entity in sourceCad.Entities)
                {
                    dxfBlock.AddEntity(entity);
                }

                // Add the new block to the target drawing's block collection
                // BlockEntities is a dictionary; add using the block name as key
                targetCad.BlockEntities.Add(dxfBlock.Name, dxfBlock);

                // Create an insert object that references the newly created block
                var insert = new CadInsertObject
                {
                    Name = dxfBlock.Name,
                    InsertionPoint = new Cad3DPoint(0, 0, 0),
                    ScaleX = 1.0,
                    ScaleY = 1.0,
                    ScaleZ = 1.0,
                    RotationAngle = 0.0
                };

                // Append the insert object to the target drawing's entities collection
                targetCad.Entities = targetCad.Entities
                    .Concat(new CadEntityBase[] { insert })
                    .ToArray();

                // Ensure the output directory exists
                var outputDir = Path.GetDirectoryName(Path.GetFullPath(resultPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified DWG file, preserving native CAD objects
                targetCad.Save(resultPath);
                Console.WriteLine($"Successfully saved result to {resultPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}