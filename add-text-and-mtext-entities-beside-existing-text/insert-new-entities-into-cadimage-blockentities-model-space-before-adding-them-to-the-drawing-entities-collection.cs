using System;
using System.IO;
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
            using (Image cadImg = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)cadImg;

                // Create a new line entity
                CadLine newLine = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 100, 0),
                    LayerName = "0"
                };

                // Insert the line into the MODEL_SPACE block first
                if (cadImage.BlockEntities.TryGetValue("*MODEL_SPACE", out var blockObj) &&
                    blockObj is CadBlockEntity modelSpaceBlock)
                {
                    // The Entities collection of CadBlockEntity supports Add
                    modelSpaceBlock.Entities.Add(newLine);
                }
                else
                {
                    Console.Error.WriteLine("MODEL_SPACE block not found.");
                    return;
                }

                // Optionally, also add the line to the drawing's top‑level entities collection
                // (If the Entities property supports Add; otherwise this step is skipped)
                if (cadImage.Entities is System.Collections.IList entityList)
                {
                    entityList.Add(newLine);
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