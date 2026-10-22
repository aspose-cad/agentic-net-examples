using System;
using System.IO;
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
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Create a new block entity
                CadBlockEntity newBlock = new CadBlockEntity
                {
                    BasePoint = new Cad3DPoint(0, 0, 0)
                };

                // Create a line entity to place inside the block
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0)
                };

                // Add the line to the block's entity collection
                newBlock.AddEntity(line);

                // Add the block to the drawing's block dictionary
                const string blockName = "MyBlock";
                cadImage.BlockEntities.Add(blockName, newBlock);

                // Validate that the block was successfully added
                if (cadImage.BlockEntities.ContainsKey(blockName))
                {
                    CadBlockEntity retrievedBlock = cadImage.BlockEntities[blockName];
                    Console.WriteLine($"Block '{blockName}' successfully added. It contains {retrievedBlock.Entities.Count} entity(ies).");
                }
                else
                {
                    Console.WriteLine($"Block '{blockName}' was not found in the block dictionary.");
                }

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