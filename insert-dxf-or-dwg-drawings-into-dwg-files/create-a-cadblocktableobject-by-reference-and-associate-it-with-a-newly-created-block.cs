using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "template.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Create a new block definition
                var newBlock = new CadBlockEntity();
                newBlock.Name = "MyBlock";

                // Add a line entity to the block
                var line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0)
                };
                newBlock.AddEntity(line);

                // Add the block definition to the drawing's block table
                cadImage.BlockEntities.Add(newBlock.Name, newBlock);

                // Create a block reference (insert) to place the block in the drawing
                var blockInsert = new CadInsertObject
                {
                    Name = newBlock.Name, // reference the block by name
                    InsertionPoint = new Cad3DPoint(50, 50, 0),
                    ScaleX = 1,
                    ScaleY = 1,
                    ScaleZ = 1
                };

                // Add the insert object to the drawing's top-level entities
                cadImage.Entities = cadImage.Entities
                    .Concat(new CadEntityBase[] { blockInsert })
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