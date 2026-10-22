using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Blocks;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        // Input and output file paths
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        try
        {
            // Load the existing CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a new block definition
                CadBlockEntity newBlock = new CadBlockEntity
                {
                    Name = "MyBlock",
                    BasePoint = new Cad3DPoint(0, 0, 0)
                };

                // Add a line entity to the block
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0)
                };
                newBlock.AddEntity(line);

                // Register the block in the drawing's block collection
                cadImage.BlockEntities.Add(newBlock.Name, newBlock);

                // Retrieve the associated block table record
                CadBlockTableObject blockTable = newBlock.CadBlockTableObject;

                // Verify that the block table object correctly links to the block
                if (blockTable != null && blockTable.BlockName == newBlock.Name)
                {
                    Console.WriteLine("Block table object correctly linked to the block.");
                }
                else
                {
                    Console.WriteLine("Block table linkage verification failed.");
                }

                // Save the modified drawing (binary DXF format as an example)
                DxfOptions saveOptions = new DxfOptions
                {
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}