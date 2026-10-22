using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;

class ChangeHatchColor
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (CadImage image = (CadImage)Image.Load(inputPath))
            {
                foreach (var layout in image.Layouts.Values)
                {
                    foreach (var entity in layout.Entities)
                    {
                        if (entity is CadHatch hatch)
                        {
                            // Set hatch color to true‑color blue (RGB 0,0,255)
                            hatch.ColorValue = 0x0000FF;
                            // Alternatively, use indexed color:
                            // hatch.ColorId = 5; // AutoCAD color index for blue
                        }
                    }
                }

                image.Save(outputPath);
                Console.WriteLine($"Hatch colors updated and saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}