using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddBackgroundImagesToDXF
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Paths to the source DXF files and the output file
                string backgroundDxfPath = "background.dxf";
                string insertDxfPath = "insert.dxf";
                string outputDxfPath = "result.dxf";

                // Verify input files exist
                if (!File.Exists(backgroundDxfPath) || !File.Exists(insertDxfPath))
                {
                    Console.Error.WriteLine("One or both input DXF files were not found.");
                    return;
                }

                // Load the background DXF (which already contains a background image) and the DXF to be inserted
                using (Image backgroundImage = Image.Load(backgroundDxfPath))
                using (Image insertImage = Image.Load(insertDxfPath))
                {
                    // Cast loaded images to CadImage to access CAD-specific members
                    CadImage backgroundCad = (CadImage)backgroundImage;
                    CadImage insertCad = (CadImage)insertImage;

                    // Define a unique name for the new block that will hold the inserted drawing
                    string newBlockName = "InsertedBlock";

                    // Create a new block entity and copy entities from the insert drawing into it
                    CadBlockEntity newBlock = new CadBlockEntity
                    {
                        BasePoint = new Cad3DPoint(0, 0, 0)
                    };

                    foreach (var entity in insertCad.Entities)
                    {
                        newBlock.AddEntity(entity);
                    }

                    // Add the new block to the background drawing's block collection
                    backgroundCad.BlockEntities.Add(newBlockName, newBlock);

                    // Create an insert object that references the newly added block
                    CadInsertObject insertObject = new CadInsertObject
                    {
                        Name = newBlockName,                     // Reference to the block name
                        InsertionPoint = new Cad3DPoint(100, 100, 0),   // Position where the block will be placed
                        ScaleX = 1.0f,
                        ScaleY = 1.0f,
                        ScaleZ = 1.0f,
                        RotationAngle = 0.0
                    };

                    // Add the insert object to the background drawing's entity collection
                    backgroundCad.Entities = backgroundCad.Entities
                        .Concat(new CadEntityBase[] { insertObject })
                        .ToArray();

                    // Save the combined drawing as a DXF file
                    DxfOptions saveOptions = new DxfOptions();
                    backgroundCad.Save(outputDxfPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}
