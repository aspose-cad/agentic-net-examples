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
        const string inputPath = "input.dxf";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Find the first hatch entity
            CadHatch hatch = cadImage.Entities.OfType<CadHatch>().FirstOrDefault();

            if (hatch != null)
            {
                // Adjust the hatch angle to 45 degrees
                hatch.HatchAngle = 45.0;
                Console.WriteLine("Hatch angle set to 45 degrees.");
            }
            else
            {
                Console.WriteLine("No hatch entity found in the drawing.");
            }

            // Save the modified drawing
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };
            cadImage.Save(outputPath, dxfOptions);
            Console.WriteLine($"Modified drawing saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}