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
        try
        {
            // Path to the source DWG file
            string sourcePath = "input.dwg";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the DWG drawing
            using (Image dwgImage = Image.Load(sourcePath))
            {
                // Configure DXF save options to preserve editable objects
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,      // Preserve objects
                    Version = DxfOutputVersion.R12,          // DXF R12 version
                    DxfFileFormat = CadFileFormat.Ascii      // ASCII encoding
                };

                // Path for the output DXF file
                string outputPath = "output.dxf";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the drawing as DXF
                dwgImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}