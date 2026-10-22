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
                // Load an existing CAD file
                string inputPath = "input.dxf";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        CadImage cadImage = (CadImage)Image.Load(inputPath);

                // NOTE: PenOptions only supports StartCap and EndCap.
                // It does NOT provide properties for color, dash style, line join, etc.
                // Therefore distinct colors or dash patterns per entity cannot be set via PenOptions.
                // As an alternative, line thickness can be controlled with LineScale.

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Millimeter;
                rasterOptions.PageWidth = 210;   // A4 width in mm
                rasterOptions.PageHeight = 297;  // A4 height in mm
                rasterOptions.LineScale = 0.5f;  // Adjust line thickness

                // Set PenOptions with different caps (example usage)
                PenOptions penOptions = new PenOptions();
                penOptions.StartCap = LineCap.Round;
                penOptions.EndCap = LineCap.Square;
                rasterOptions.PenOptions = penOptions;

                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save the rendered image
                string outputPath = "output.png";
                cadImage.Save(outputPath, pngOptions);

                // Clean up
                cadImage.Dispose();
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}