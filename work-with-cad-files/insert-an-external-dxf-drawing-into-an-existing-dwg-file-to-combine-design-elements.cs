using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string basePath = "BaseDrawing.dwg";
        const string externalPath = "ExternalDrawing.dxf";
        const string outputPath = "CombinedDrawing.dwg";

        if (!File.Exists(basePath))
        {
            Console.Error.WriteLine($"Base drawing not found: {basePath}");
            return;
        }

        if (!File.Exists(externalPath))
        {
            Console.Error.WriteLine($"External drawing not found: {externalPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the base DWG drawing
            using (Image baseImage = Image.Load(basePath))
            {
                // Load the external DXF drawing
                using (Image externalImage = Image.Load(externalPath))
                {
                    // Cast to CadImage to work with CAD-specific members
                    CadImage baseCad = (CadImage)baseImage;
                    CadImage externalCad = (CadImage)externalImage;

                    // Append entities from the DXF drawing to the DWG drawing
                    baseCad.Entities = baseCad.Entities.Concat(externalCad.Entities).ToArray();

                    // Save the combined drawing as a new DWG file
                    baseCad.Save(outputPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}