using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using System.IO;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source CAD drawing
                    string inputPath = "input.dwg";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the exported DXF file
                    string outputPath = "output.dxf";

                    // Load the CAD drawing
                    using (Aspose.CAD.Image cadImage = Aspose.CAD.Image.Load(inputPath))
                    {
                        // Configure DXF export options with binary encoding
                        DxfOptions dxfOptions = new DxfOptions();
                        dxfOptions.OutputMode = CadOutputMode.Convert;          // Preserve editable objects
                        dxfOptions.Version = DxfOutputVersion.R12;              // DXF version R12
                        dxfOptions.DxfFileFormat = CadFileFormat.Binary;       // Binary DXF encoding

                        // Save the drawing as DXF using the configured options
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
}