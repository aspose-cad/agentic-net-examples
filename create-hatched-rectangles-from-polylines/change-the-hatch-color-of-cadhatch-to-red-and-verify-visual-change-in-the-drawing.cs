using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

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
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                foreach (var hatch in cadImage.Entities.OfType<CadHatch>())
                {
                    hatch.ColorValue = 0xFF0000; // Red true color
                    hatch.ColorId = 0; // Clear indexed color
                }

                bool redHatchFound = cadImage.Entities
                    .OfType<CadHatch>()
                    .Any(h => h.ColorValue == 0xFF0000);

                Console.WriteLine(redHatchFound
                    ? "Hatch color successfully changed to red."
                    : "No hatch color was changed.");

                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };

                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}