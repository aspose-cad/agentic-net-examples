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
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                CadInsertObject insertObject = null;
                foreach (CadEntityBase topEntity in cadImage.Entities)
                {
                    if (topEntity is CadInsertObject insert)
                    {
                        insertObject = insert;
                        break;
                    }
                }

                if (insertObject != null)
                {
                    string referencedBlockName = insertObject.Name;

                    CadBlockEntity blockDefinition = null;
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        if (block.Name == referencedBlockName)
                        {
                            blockDefinition = block;
                            break;
                        }
                    }

                    if (blockDefinition != null)
                    {
                        foreach (CadEntityBase blockEntity in blockDefinition.Entities)
                        {
                            blockEntity.LayerName = "ModifiedLayer";
                        }
                    }
                }

                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}