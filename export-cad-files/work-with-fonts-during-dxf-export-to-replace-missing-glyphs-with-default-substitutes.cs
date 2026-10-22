using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source DXF file
                string inputPath = "input.dxf";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the exported DXF file
                string outputPath = "output.dxf";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cadImage = (CadImage)image;

                    // Set a default font to be used when a glyph is missing in the source drawing
                    cadImage.DefaultFont = "Arial";

                    // Configure DXF save options to preserve editable objects (convert mode)
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };

                    // Save the drawing with the specified options
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