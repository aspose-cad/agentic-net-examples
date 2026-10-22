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
        const string inputPath = "input.dxf";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // NOTE: The Aspose.CAD SDK does not expose a CadCache class or an ExactReallocateOnly property.
            // Therefore, cache reallocation control cannot be set via code.

            // Load the DXF file
            using (Image dxfImage = Image.Load(inputPath))
            {
                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Prepare DXF save options: preserve editable objects (Convert mode)
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    Version = DxfOutputVersion.R12,
                    DxfFileFormat = CadFileFormat.Binary // or CadFileFormat.Ascii
                };

                // Save the image as a DXF file
                dxfImage.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}