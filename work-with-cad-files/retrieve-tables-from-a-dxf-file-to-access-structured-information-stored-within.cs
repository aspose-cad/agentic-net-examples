using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dxf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(sourcePath))
            {
                CadImage cadImage = (CadImage)image;

                // The Aspose.CAD API does not expose DXF tables (layers, linetypes) directly.
                // Retrieving such structured information is not supported in this version.
                Console.WriteLine("Retrieving DXF tables (layers, linetypes) is not available in the current Aspose.CAD API.");

                // Example: Save the DXF preserving editable objects (optional)
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    Version = DxfOutputVersion.R12,
                    DxfFileFormat = CadFileFormat.Binary
                };

                string outputPath = "output.dxf";
                string outDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                cadImage.Save(outputPath, saveOptions);
                Console.WriteLine($"File saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}