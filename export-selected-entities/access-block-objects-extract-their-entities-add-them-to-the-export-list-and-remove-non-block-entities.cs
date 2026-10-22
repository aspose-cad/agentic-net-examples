using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

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

                List<CadEntityBase> exportEntities = new List<CadEntityBase>();

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        exportEntities.Add(entity);
                    }
                }

                // Replace top‑level entities with the collected block entities
                cadImage.Entities = exportEntities.ToArray();

                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}