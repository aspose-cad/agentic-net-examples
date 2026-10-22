using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

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
            // Load an existing DWG file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to work with CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Define a name for the new block
                string blockName = "MyBlock";

                // Create a new CadBlockEntity
                CadBlockEntity newBlock = new CadBlockEntity
                {
                    BasePoint = new Cad3DPoint(0, 0, 0)
                };

                // Add a line entity to the block
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0),
                    LayerName = "0"
                };
                newBlock.AddEntity(line);

                // Add the block to the drawing's block collection
                cadImage.BlockEntities.Add(blockName, newBlock);

                // Create an insert object that references the block
                CadInsertObject insert = new CadInsertObject
                {
                    Name = blockName, // Block reference
                    InsertionPoint = new Cad3DPoint(200, 200, 0),
                    ScaleX = 1,
                    ScaleY = 1,
                    ScaleZ = 1
                };

                // Add the insert object to the top-level entities
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { insert })
                    .ToArray();

                // Save the modified drawing to a new DWG file
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}