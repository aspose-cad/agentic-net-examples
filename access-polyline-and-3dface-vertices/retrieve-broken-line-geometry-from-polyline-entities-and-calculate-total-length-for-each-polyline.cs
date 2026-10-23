using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dwg";

        if (!File.Exists(inputPath))
        {
 Console.Error.WriteLine("Input CAD file not found: " + inputPath);
 return;
}

        try
        {
            using var cadImage = (CadImage)Image.Load(inputPath);

            int polylineCount = 0;
            int faceCount = 0;

            foreach (var entity in cadImage.Entities)
            {
                switch (entity)
                {
                    case CadLwPolyline lwPolyline:
                        polylineCount++;
                        ProcessLwPolyline(lwPolyline, polylineCount);
                        break;

                    case CadPolylineBase polyline3D:
                        polylineCount++;
                        Process3DPolyline(polyline3D, polylineCount);
                        break;

                    case Cad3DFace face:
                        faceCount++;
                        Process3DFace(face, faceCount);
                        break;

                    default:
                        // Unhandled entity type
                        break;
                }
            }

            if (polylineCount == 0 && faceCount == 0)
            {
                Console.WriteLine("No polyline or 3DFace entities were found in the CAD file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Processing failed: {ex.Message}");
        }
    }

    private static void ProcessLwPolyline(CadLwPolyline polyline, int index)
    {
        var points = polyline.Coordinates
            .Select(p => new Point2D(p.X, p.Y))
            .ToList();

        bool isClosed = polyline.Flag == CadLwPolylineFlag.Closed;

        double length = CalculatePolylineLength(points, isClosed);

        Console.WriteLine($"Polyline #{index} (2D) Length: {length:F3}");
        for (int i = 0; i < points.Count; i++)
        {
            var p = points[i];
            Console.WriteLine($"  Vertex {i + 1}: ({p.X}, {p.Y})");
        }
    }

    private static void Process3DPolyline(CadPolylineBase polyline, int index)
    {
        var vertices = polyline.ChildObjects
            .OfType<CadVertexBase>()
            .Select(v => v.LocationPoint)
            .ToList();

        bool isClosed = polyline.Flag == CadPolylineFlag.CLOSED_POLY;

        double length = Calculate3DPolylineLength(vertices, isClosed);

        Console.WriteLine($"Polyline #{index} (3D) Length: {length:F3}");
        for (int i = 0; i < vertices.Count; i++)
        {
            var v = vertices[i];
            Console.WriteLine($"  Vertex {i + 1}: ({v.X}, {v.Y}, {v.Z})");
        }
    }

    private static void Process3DFace(Cad3DFace face, int index)
    {
        Console.WriteLine($"3DFace #{index} Corners:");
        Print3DPoint("  First", face.FirstCorner);
        Print3DPoint("  Second", face.SecondCorner);
        Print3DPoint("  Third", face.ThirdCorner);
        Print3DPoint("  Fourth", face.FourthCorner);
    }

    private static double CalculatePolylineLength(IList<Point2D> points, bool isClosed)
    {
        double total = 0.0;
        if (points == null || points.Count < 2)
            return total;

        for (int i = 0; i < points.Count - 1; i++)
        {
            var p1 = points[i];
            var p2 = points[i + 1];
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            total += Math.Sqrt(dx * dx + dy * dy);
        }

        if (isClosed && points.Count > 1)
        {
            var first = points[0];
            var last = points[points.Count - 1];
            double dx = first.X - last.X;
            double dy = first.Y - last.Y;
            total += Math.Sqrt(dx * dx + dy * dy);
        }

        return total;
    }

    private static double Calculate3DPolylineLength(IList<Cad3DPoint> points, bool isClosed)
    {
        double total = 0.0;
        if (points == null || points.Count < 2)
            return total;

        for (int i = 0; i < points.Count - 1; i++)
        {
            var p1 = points[i];
            var p2 = points[i + 1];
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double dz = p2.Z - p1.Z;
            total += Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        if (isClosed && points.Count > 1)
        {
            var first = points[0];
            var last = points[points.Count - 1];
            double dx = first.X - last.X;
            double dy = first.Y - last.Y;
            double dz = first.Z - last.Z;
            total += Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        return total;
    }

    private static void Print3DPoint(string label, Cad3DPoint point)
    {
        Console.WriteLine($"{label}: ({point.X}, {point.Y}, {point.Z})");
    }
}