using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace InsertDrawingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths
            string sourcePath = "source.dwg";   // Drawing to be inserted
            string targetPath = "target.dwg";   // Drawing that will receive the insertion
            string outputPath = "output.dwg";   // Resulting drawing

            // Validate input files
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            if (!File.Exists(targetPath))
            {
                Console.Error.WriteLine($"Target file not found: {targetPath}");
                return;
            }

            try
            {
                // Load source and target drawings
                using (Image srcImage = Image.Load(sourcePath))
                using (Image tgtImage = Image.Load(targetPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage srcCad = srcImage as CadImage;
                    CadImage tgtCad = tgtImage as CadImage;

                    if (srcCad == null || tgtCad == null)
                    {
                        Console.Error.WriteLine("One of the files could not be loaded as a CAD image.");
                        return;
                    }

                    // Create a new block that will hold the source drawing's entities
                    CadBlockEntity insertedBlock = new CadBlockEntity
                    {
                        Name = "InsertedDrawing"
                    };

                    // Copy entities from the source drawing into the new block
                    foreach (CadEntityBase entity in srcCad.Entities)
                    {
                        // Directly add the entity; in real scenarios cloning may be required
                        insertedBlock.AddEntity(entity);
                    }

                    // Add the new block definition to the target drawing
                    // BlockEntities is a dictionary<string, CadBlockEntity>
                    tgtCad.BlockEntities.Add(insertedBlock.Name, insertedBlock);

                    // Create an insert object that references the new block
                    CadInsertObject insertObj = new CadInsertObject
                    {
                        InsertionPoint = new Cad3DPoint(0, 0, 0),
                        Name = insertedBlock.Name,
                        ScaleX = 1.0,
                        ScaleY = 1.0,
                        ScaleZ = 1.0,
                        RotationAngle = 0.0
                    };

                    // Add the insert object to the target drawing's top‑level entities
                    tgtCad.Entities = tgtCad.Entities
                        .Concat(new CadEntityBase[] { insertObj })
                        .ToArray();

                    // Save the modified drawing
                    tgtCad.Save(outputPath);
                }

                Console.WriteLine($"Drawing saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}