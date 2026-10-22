using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace InsertDwginDxf
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input file paths (replace with actual paths)
                string dwgPath = "source.dwg";
                string dxfPath = "target.dxf";
                string outputPath = "combined.dxf";

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

                // Load DWG and DXF images
                using (CadImage dwgImage = (CadImage)Image.Load(dwgPath))
                using (CadImage dxfImage = (CadImage)Image.Load(dxfPath))
                {
                    // Combine entities from both drawings
                    var combinedEntities = dxfImage.Entities
                        .Concat(dwgImage.Entities)
                        .ToArray();

                    // Replace the DXF image's entities with the combined set
                    dxfImage.Entities = combinedEntities;

                    // Retrieve and display combined layout information
                    foreach (var layoutKvp in dxfImage.Layouts)
                    {
                        string layoutName = layoutKvp.Key;
                        Console.WriteLine($"Layout \"{layoutName}\" found in combined drawing.");
                    }

                    // Save the combined drawing as DXF
                    DxfOptions dxfOptions = new DxfOptions();
                    dxfImage.Save(outputPath, dxfOptions);
                    Console.WriteLine($"Combined drawing saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}