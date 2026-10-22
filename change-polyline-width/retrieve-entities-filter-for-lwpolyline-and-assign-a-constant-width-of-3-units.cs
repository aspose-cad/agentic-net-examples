using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.svg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Retrieve all LWPOLYLINE entities
                var lwPolylines = cadImage.Entities
                    .OfType<CadLwPolyline>()
                    .ToArray();

                // Set a constant width of 3 units for rendering
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 3f,
                    PageWidth = 800,
                    PageHeight = 600,
                    AutomaticLayoutsScaling = true,
                    Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };

                var svgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, svgOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}