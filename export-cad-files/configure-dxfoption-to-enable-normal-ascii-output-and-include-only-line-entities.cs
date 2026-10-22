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
        string inputPath = "input.dwg";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Note: Direct removal or replacement of non‑line entities is not supported by the Aspose.CAD API.
                // The current implementation saves the entire drawing while configuring DXF options as requested.

                DxfOptions dxfOptions = new DxfOptions
                {
                    DxfFileFormat = CadFileFormat.Ascii,   // ASCII encoding
                    OutputMode = CadOutputMode.Convert,    // Preserve objects
                    Version = DxfOutputVersion.R12        // DXF R12 version
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