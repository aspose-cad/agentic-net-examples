using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

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
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Create a line entity
                var line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 0, 0),
                    LayerName = "0"
                };

                // Add the line to the drawing
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { line }).ToArray();

                // Create a lightweight polyline entity (a rectangle)
                var polyline = new CadLwPolyline
                {
                    LayerName = "0",
                    Flag = CadLwPolylineFlag.Closed,
                    Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(0, 100),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(100, 0)
                    }
                };

                // Add the polyline to the drawing
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { polyline }).ToArray();

                // Save the modified drawing as a DXF file
                var saveOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}