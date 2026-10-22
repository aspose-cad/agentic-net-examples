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
            // Load an existing DWG drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Create a new block definition
                CadBlockEntity newBlock = new CadBlockEntity
                {
                    BasePoint = new Cad3DPoint(0, 0, 0)
                };

                // Create a line entity to place inside the block
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0),
                    LayerName = "0"
                };

                // Add the line to the block's entity collection
                newBlock.AddEntity(line);

                // Register the block in the drawing with a name
                string blockName = "MyBlock";
                cadImage.BlockEntities.Add(blockName, newBlock);

                // Create an insert object to place the block in the drawing
                CadInsertObject insert = new CadInsertObject
                {
                    Name = blockName, // Reference the block by name
                    InsertionPoint = new Cad3DPoint(200, 200, 0),
                    ScaleX = 1,
                    ScaleY = 1,
                    ScaleZ = 1,
                    LayerName = "0"
                };

                // Add the insert object to the top-level entities collection
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { insert })
                    .ToArray();

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