using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
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
        // Path for the exported DXF file
                    string outputPath = "output.dxf";

                    // Load the source CAD drawing
                    using (Image loadedImage = Image.Load(inputPath))
                    {
                        // Cast to CadImage to work with CAD-specific features (e.g., block hierarchy)
                        CadImage cadImage = (CadImage)loadedImage;

                        // Create DXF export options – no special rasterization needed for vector output
                        DxfOptions dxfOptions = new DxfOptions();

                        // Save the drawing to DXF format, preserving block definitions and hierarchy
                        cadImage.Save(outputPath, dxfOptions);
                    }

                    Console.WriteLine("CAD drawing exported to DXF successfully.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}