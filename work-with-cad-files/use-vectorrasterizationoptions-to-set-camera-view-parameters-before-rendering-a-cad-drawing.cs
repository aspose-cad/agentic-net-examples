using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats;
using Aspose.CAD.ImageOptions;

class Program
{
    static int Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output/camera-view.pdf";
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine("Input file not found: " + inputPath);
            return 0;
        }
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath)));
            using (Image drawing = Image.Load(inputPath))
            {
                // ObserverPoint accepts rotation angles, not Cartesian camera coordinates.
                var raster = new CadRasterizationOptions
                {
                    PageWidth = 1200,
                    PageHeight = 900,
                    BackgroundColor = Aspose.CAD.Color.White,
                    ObserverPoint = new ObserverPoint(10f, 20f, 30f),
                    Zoom = 0.9f
                };
                // The export property accepts the abstract base type; construct its CAD subtype.
                VectorRasterizationOptions vectorOptions = raster;
                var pdf = new PdfOptions { VectorRasterizationOptions = vectorOptions };
                drawing.Save(outputPath, pdf);
                Console.WriteLine("Exported CAD view: " + outputPath);
            }
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("CAD rendering failed: " + ex.Message);
            return 1;
        }
    }
}