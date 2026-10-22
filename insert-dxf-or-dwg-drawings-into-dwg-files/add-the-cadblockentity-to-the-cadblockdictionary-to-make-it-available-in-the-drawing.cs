using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

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

                // Add a line entity to the block
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0)
                };
                newBlock.AddEntity(line);

                // Add the block to the drawing's block dictionary
                string blockName = "MyNewBlock";
                cadImage.BlockEntities.Add(blockName, newBlock);

                // Insert the newly created block into the drawing
                CadInsertObject insert = new CadInsertObject
                {
                    Name = blockName,                     // Reference to the block name
                    InsertionPoint = new Cad3DPoint(10, 10, 0),
                    ScaleX = 1,
                    ScaleY = 1,
                    ScaleZ = 1
                };
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