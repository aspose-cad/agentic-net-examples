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

        // Ensure output directory exists
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            var cadImage = (CadImage)Image.Load(inputPath);

            // Find the first hatch entity
            var hatch = cadImage.Entities.OfType<CadHatch>().FirstOrDefault();
            if (hatch != null)
            {
                // Set hatch angle to 45 degrees (in radians)
                hatch.HatchAngle = Math.PI / 4;

                // Set hatch color using an indexed color (ACI 1 = red)
                hatch.ColorId = 1;

                // Alternatively, set a true color (red) using ColorValue
                // hatch.ColorValue = 0xFF0000;
            }
            else
            {
                Console.Error.WriteLine("No hatch entity found in the drawing.");
            }

            // Save the modified drawing with explicit DXF options
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };
            cadImage.Save(outputPath, dxfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}