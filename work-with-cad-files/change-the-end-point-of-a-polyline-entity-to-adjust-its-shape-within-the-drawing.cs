using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class ModifyPolyline
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
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // 2D Lightweight Polyline (LWPOLYLINE) handling
                CadLwPolyline lwPolyline = cadImage.Entities
                    .OfType<CadLwPolyline>()
                    .FirstOrDefault();

                if (lwPolyline != null && lwPolyline.PointCount > 0)
                {
                    int lastIndex = lwPolyline.PointCount - 1;
                    // New end point coordinates (replace with desired values)
                    Cad2DPoint newEndPoint = new Cad2DPoint(150.0, 75.0);
                    lwPolyline.Coordinates[lastIndex] = newEndPoint;
                }

                // 3D Polyline (POLYLINE) handling
                CadPolyline poly3D = cadImage.Entities
                    .OfType<CadPolyline>()
                    .FirstOrDefault();

                if (poly3D != null)
                {
                    List<CadVertexBase> vertices = poly3D.ChildObjects
                        .OfType<CadVertexBase>()
                        .ToList();

                    if (vertices.Count > 0)
                    {
                        CadVertexBase lastVertex = vertices[vertices.Count - 1];
                        // New end point coordinates (replace with desired values)
                        Cad3DPoint newEndPoint3D = new Cad3DPoint(150.0, 75.0, 20.0);
                        lastVertex.LocationPoint = newEndPoint3D;
                    }
                }

                // Save the modified drawing
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };

                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}