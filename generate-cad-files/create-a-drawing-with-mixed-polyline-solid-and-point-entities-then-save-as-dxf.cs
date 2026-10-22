using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Create a new empty CAD image
        CadImage cadImage = new CadImage();

        // ---------- Create a 2D closed polyline ----------
        CadLwPolyline polyline2D = new CadLwPolyline();
        polyline2D.Coordinates.Add(new Cad2DPoint(0, 0));
        polyline2D.Coordinates.Add(new Cad2DPoint(100, 0));
        polyline2D.Coordinates.Add(new Cad2DPoint(100, 100));
        polyline2D.Coordinates.Add(new Cad2DPoint(0, 100));
        polyline2D.Flag = CadLwPolylineFlag.Closed; // close the polyline

        // ---------- Create a SOLID entity (using 3DFace) ----------
        Cad3DFace solidFace = new Cad3DFace(
            new Cad3DPoint(150, 0, 0),
            new Cad3DPoint(250, 0, 0),
            new Cad3DPoint(250, 100, 0),
            new Cad3DPoint(150, 100, 0));

        // ---------- Create a POINT entity ----------
        CadPoint point = new CadPoint();
        point.CenterPoint = new Cad3DPoint(300, 50, 0);

        // Append the new entities to the image.
        cadImage.Entities = cadImage.Entities
            .Concat(new CadEntityBase[] { polyline2D, solidFace, point })
            .ToArray();

        // Prepare output path and ensure directory exists
        string outputPath = Path.Combine("output", "MixedEntities.dxf");
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Save the drawing as DXF (ASCII format, preserve objects)
        var dxfOptions = new DxfOptions
        {
            DxfFileFormat = CadFileFormat.Ascii,
            OutputMode = CadOutputMode.Convert,
            Version = DxfOutputVersion.R12
        };

        try
        {
            cadImage.Save(outputPath, dxfOptions);
            Console.WriteLine($"DXF file saved successfully to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving DXF: {ex.Message}");
        }
    }
}