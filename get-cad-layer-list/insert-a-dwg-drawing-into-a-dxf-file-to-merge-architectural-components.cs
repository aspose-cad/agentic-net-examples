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
        try
        {
            // Placeholder file paths
            string dwgPath = "source.dwg";
            string dxfPath = "target.dxf";
            string outputPath = "merged.dxf";

            // Verify input files exist
            if (!File.Exists(dwgPath))
            {
                Console.Error.WriteLine($"DWG file not found: {dwgPath}");
                return;
            }
            if (!File.Exists(dxfPath))
            {
                Console.Error.WriteLine($"DXF file not found: {dxfPath}");
                return;
            }

            // Load source DWG and target DXF images
            using (Image dwgImage = Image.Load(dwgPath))
            using (Image dxfImage = Image.Load(dxfPath))
            {
                CadImage sourceCad = dwgImage as CadImage;
                CadImage targetCad = dxfImage as CadImage;

                if (sourceCad == null || targetCad == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded images to CadImage.");
                    return;
                }

                // Append entities from the DWG into the DXF
                foreach (CadEntityBase entity in sourceCad.Entities)
                {
                    targetCad.Entities = targetCad.Entities.Concat(new[] { entity }).ToArray();
                }

                // Save the merged DXF file
                targetCad.Save(outputPath, new DxfOptions());
                Console.WriteLine($"Merged DXF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}