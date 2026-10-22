using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' not found.");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Locate the block definition named "MyBlock"
                CadBlockEntity targetBlock = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "MyBlock")
                    {
                        targetBlock = block;
                        break;
                    }
                }

                if (targetBlock == null)
                {
                    Console.WriteLine("Block 'MyBlock' not found.");
                    return;
                }

                // Move every insert reference of the block by an offset of (100, 0, 0)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadInsertObject insert && insert.OriginalBlockName == "MyBlock")
                    {
                        Cad3DPoint oldPoint = insert.InsertionPoint;
                        Cad3DPoint newPoint = new Cad3DPoint(oldPoint.X + 100.0, oldPoint.Y, oldPoint.Z);
                        insert.InsertionPoint = newPoint;
                    }
                }

                // Prepare rasterization options to reflect the updated layout
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                // Save the modified drawing as PNG
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Modified drawing saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}