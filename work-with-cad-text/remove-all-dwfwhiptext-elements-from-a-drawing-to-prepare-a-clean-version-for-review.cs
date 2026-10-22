using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
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
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // The Aspose.CAD SDK does not expose a CadEntityTypeName value for DwfWhipText.
                // Therefore, removing DwfWhipText entities cannot be performed with the current API.
                Console.WriteLine("Warning: DwfWhipText entity type is not supported by the SDK; no entities were removed.");

                // Save the (unchanged) drawing as DXF (ASCII format)
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii,
                    Version = DxfOutputVersion.R12
                };

                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"Drawing saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}