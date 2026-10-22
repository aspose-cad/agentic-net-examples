using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityInsertionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file \"{inputPath}\" not found.");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Find the "*MODEL_SPACE" block definition
                    CadBlockEntity modelSpaceBlock = cadImage.BlockEntities.ValuesTyped
                        .FirstOrDefault(b => b.Name == "*MODEL_SPACE");

                    if (modelSpaceBlock == null)
                    {
                        Console.WriteLine("The *MODEL_SPACE block was not found.");
                        return;
                    }

                    // Create a new line entity
                    CadLine newLine = new CadLine
                    {
                        FirstPoint = new Cad3DPoint(0.0, 0.0, 0.0),
                        SecondPoint = new Cad3DPoint(100.0, 100.0, 0.0)
                    };

                    // Insert the new line into the *MODEL_SPACE block
                    if (modelSpaceBlock.Entities is IList<CadEntityBase> blockEntityList)
                    {
                        blockEntityList.Add(newLine);
                    }
                    else
                    {
                        Console.WriteLine("Unable to add entity to the block's entity collection.");
                    }

                    // Also add the new line to the drawing's top‑level entities collection
                    if (cadImage.Entities is IList<CadEntityBase> topEntityList)
                    {
                        topEntityList.Add(newLine);
                    }
                    else
                    {
                        Console.WriteLine("Unable to add entity to the drawing's top‑level entity collection.");
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                    Console.WriteLine($"Drawing saved to \"{outputPath}\".");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}