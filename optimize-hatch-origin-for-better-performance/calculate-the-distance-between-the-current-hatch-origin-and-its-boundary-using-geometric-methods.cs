using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class HatchOriginDistanceCalculator
{
    // Calculates the shortest distance from a point to a polyline defined by a list of vertices.
    private static double PointToPolylineDistance(Point2D point, IList<Point2D> vertices)
    {
        double minDist = double.MaxValue;
        int count = vertices.Count;

        for (int i = 0; i < count; i++)
        {
            Point2D a = vertices[i];
            Point2D b = vertices[(i + 1) % count];

            double vx = b.X - a.X;
            double vy = b.Y - a.Y;

            double wx = point.X - a.X;
            double wy = point.Y - a.Y;

            double c1 = vx * wx + vy * wy;
            double c2 = vx * vx + vy * vy;

            double t = c2 != 0 ? c1 / c2 : 0;
            t = Math.Max(0, Math.Min(1, t));

            double cx = a.X + t * vx;
            double cy = a.Y + t * vy;

            double dx = point.X - cx;
            double dy = point.Y - cy;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist < minDist)
                minDist = dist;
        }

        return minDist;
    }

    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                CadHatch hatch = cadImage.Entities
                    .OfType<CadHatch>()
                    .FirstOrDefault();

                if (hatch == null)
                {
                    Console.WriteLine("No hatch entity found in the drawing.");
                    return;
                }

                // Determine hatch origin (pattern origin)
                Point2D hatchOrigin = new Point2D(0, 0);
                if (hatch.PatternDefinitions != null && hatch.PatternDefinitions.Count > 0)
                {
                    var firstPattern = hatch.PatternDefinitions[0];
                    if (firstPattern != null && firstPattern.LineBasePoint != null)
                    {
                        // Convert Cad2DPoint to Point2D
                        hatchOrigin = new Point2D(firstPattern.LineBasePoint.X, firstPattern.LineBasePoint.Y);
                    }
                }

                // Retrieve boundary polyline vertices
                IList<Point2D> boundaryVertices = null;

                if (hatch.BoundaryPaths != null)
                {
                    foreach (var container in hatch.BoundaryPaths)
                    {
                        if (container?.BoundaryPath != null)
                        {
                            foreach (var path in container.BoundaryPath)
                            {
                                var polyline = path as CadPolylineBoundaryPath;
                                if (polyline != null && polyline.Vertices != null && polyline.Vertices.Count > 2)
                                {
                                    boundaryVertices = polyline.Vertices;
                                    break;
                                }
                            }
                        }

                        if (boundaryVertices != null)
                            break;
                    }
                }

                if (boundaryVertices == null)
                {
                    Console.WriteLine("Hatch does not contain a valid polyline boundary.");
                    return;
                }

                double distance = PointToPolylineDistance(hatchOrigin, boundaryVertices);
                Console.WriteLine($"Distance from hatch origin ({hatchOrigin.X}, {hatchOrigin.Y}) to its boundary: {distance}");

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}