using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputDwgPath = "input.dwg";
            string inputDgnPath = "embedded.dgn";

            if (!File.Exists(inputDwgPath))
            {
                Console.Error.WriteLine($"DWG file not found: {inputDwgPath}");
                return;
            }

            if (!File.Exists(inputDgnPath))
            {
                Console.Error.WriteLine($"DGN file not found: {inputDgnPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Load the DGN file and modify entity colors (layer color modification not directly supported)
            using (Image dgnImage = Image.Load(inputDgnPath))
            {
                if (dgnImage is CadImage dgnCadImage)
                {
                    foreach (var entity in dgnCadImage.Entities)
                    {
                        // Set color to red using indexed color (1 = red)
                        entity.ColorId = 1;
                    }
                }

                string modifiedDgnPath = Path.Combine(outputDir, "modified.dgn");
                dgnImage.Save(modifiedDgnPath);
            }

            // Load the DWG file
            using (Image dwgImage = Image.Load(inputDwgPath))
            {
                // NOTE: Embedding a modified DGN into a DWG is not exposed via the Aspose.CAD API.
                // The DWG is saved as‑is; external references could be added manually if required.
                string outputDwgPath = Path.Combine(outputDir, "output.dwg");
                dwgImage.Save(outputDwgPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}