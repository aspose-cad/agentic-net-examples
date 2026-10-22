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
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Create a new block definition
                CadBlockEntity blockEntity = new CadBlockEntity
                {
                    Name = "MyBlock"
                };

                // Add a line entity to the block
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0),
                    LayerName = "0"
                };
                blockEntity.AddEntity(line);

                // Add the block definition to the drawing's BlockEntities dictionary
                cadImage.BlockEntities.Add(blockEntity.Name, blockEntity);

                // Insert the block into the drawing
                CadInsertObject insert = new CadInsertObject
                {
                    Name = blockEntity.Name, // Reference the block by name
                    InsertionPoint = new Cad3DPoint(200, 200, 0),
                    ScaleX = 1,
                    ScaleY = 1,
                    ScaleZ = 1
                };

                // Append the insert object to the drawing's entities
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