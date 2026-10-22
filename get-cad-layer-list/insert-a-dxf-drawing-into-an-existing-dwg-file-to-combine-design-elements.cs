using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string dwgPath = "input.dwg";
            string dxfPath = "insert.dxf";
            string outputPath = "combined.dwg";

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

            using (Image dwgImageBase = Image.Load(dwgPath))
            using (Image dxfImageBase = Image.Load(dxfPath))
            {
                CadImage dwgImage = dwgImageBase as CadImage;
                CadImage dxfImage = dxfImageBase as CadImage;

                if (dwgImage == null || dxfImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD images.");
                    return;
                }

                // Append entities from DXF to DWG
                var combinedEntities = dwgImage.Entities.Concat(dxfImage.Entities).ToArray();
                dwgImage.Entities = combinedEntities;

                // Save the combined drawing
                dwgImage.Save(outputPath, new DwgOptions());
            }

            Console.WriteLine($"Combined DWG saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}