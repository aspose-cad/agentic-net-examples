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
        string targetPath = "target.dwg";
        string externalPath = "external.dxf";
        string outputPath = "combined.dxf";

        if (!File.Exists(targetPath))
        {
            Console.Error.WriteLine($"Target file not found: {targetPath}");
            return;
        }

        if (!File.Exists(externalPath))
        {
            Console.Error.WriteLine($"External file not found: {externalPath}");
            return;
        }

        try
        {
            using (Image targetImg = Image.Load(targetPath))
            using (Image externalImg = Image.Load(externalPath))
            {
                var targetCad = targetImg as CadImage;
                var externalCad = externalImg as CadImage;

                if (targetCad == null || externalCad == null)
                {
                    Console.Error.WriteLine("Failed to load one of the CAD files as CadImage.");
                    return;
                }

                var targetBlock = targetCad.BlockEntities.ValuesTyped.FirstOrDefault();
                var externalBlock = externalCad.BlockEntities.ValuesTyped.FirstOrDefault();

                if (targetBlock == null || externalBlock == null)
                {
                    Console.Error.WriteLine("Unable to locate block entities in one of the drawings.");
                    return;
                }

                foreach (var entity in externalBlock.Entities)
                {
                    // Directly add the entity to the target block.
                    targetBlock.AddEntity(entity);
                }

                var options = new DxfOptions();
                targetCad.Save(outputPath, options);
            }

            Console.WriteLine($"Combined drawing saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}