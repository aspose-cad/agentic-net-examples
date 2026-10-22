using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadPenColorExample
{
    class Program
    {
        static void Main()
        {
        try
        {
                    // Load the CAD file
                    string inputPath = "input.dxf";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        CadImage cadImage = (CadImage)Image.Load(inputPath);

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1200;
                    rasterOptions.UnitType = UnitType.Millimeter;
                    rasterOptions.LineScale = 1.0f; // Controls exported line thickness

                    // Create PenOptions – only StartCap and EndCap are available.
                    PenOptions penOptions = new PenOptions();

                    // NOTE: PenOptions does not expose a Color property.
                    // Therefore a custom pen color cannot be set directly.
                    // The only available customizations are line caps and line scale.

                    // Example: set line caps (optional)
                    penOptions.StartCap = LineCap.Flat;
                    penOptions.EndCap = LineCap.Flat;

                    // Assign PenOptions to rasterization options
                    rasterOptions.PenOptions = penOptions;

                    // Set up PNG export options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the rasterized image
                    string outputPath = "output.png";
                    cadImage.Save(outputPath, pngOptions);
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}