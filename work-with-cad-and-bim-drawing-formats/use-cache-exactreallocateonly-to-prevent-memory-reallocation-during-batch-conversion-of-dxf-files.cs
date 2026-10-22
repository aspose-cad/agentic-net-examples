using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source CAD file (DWG, DXF, etc.)
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure DXF save options
                    DxfOptions dxfOptions = new DxfOptions();
                    dxfOptions.OutputMode = CadOutputMode.Convert; // Preserve editable objects
                    dxfOptions.Version = DxfOutputVersion.R12;     // DXF R12 version
                    dxfOptions.DxfFileFormat = CadFileFormat.Binary; // Binary DXF encoding

                    // Path for the output DXF file
                    string outputPath = "output.dxf";

                    // Save the CAD image as DXF
                    cadImage.Save(outputPath, dxfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}