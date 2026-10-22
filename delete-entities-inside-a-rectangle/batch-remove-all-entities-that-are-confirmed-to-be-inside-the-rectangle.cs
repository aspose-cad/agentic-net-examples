using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

class Program
{
    static void Main()
    {
        // Input and output paths (replace with actual paths)
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Define rectangle bounds
                double minX = 0.0;
                double minY = 0.0;
                double maxX = 100.0;
                double maxY = 100.0;

                // Iterate over each block (e.g., ModelSpace)
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Collect entities that are fully inside the rectangle
                    var toRemove = new List<CadEntityBase>();

                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (IsEntityInsideRectangle(entity, minX, minY, maxX, maxY))
                        {
                            toRemove.Add(entity);
                        }
                    }

                    // Remove the collected entities from the block
                    foreach (CadEntityBase rem in toRemove)
                    {
                        block.Entities.Remove(rem);
                    }
                }

                // Save the modified CAD file
                cadImage.Save(outputPath);
            }

            Console.WriteLine($"Modified CAD saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }

    // Determines whether a CAD entity lies completely inside the specified rectangle
    static bool IsEntityInsideRectangle(CadEntityBase obj, double minX, double minY, double maxX, double maxY)
    {
        // Line entity
        if (obj is CadLine line)
        {
            return PointInside(line.FirstPoint, minX, minY, maxX, maxY) &&
                   PointInside(line.SecondPoint, minX, minY, maxX, maxY);
        }

        // Circle entity
        if (obj is CadCircle circle)
        {
            double left   = circle.CenterPoint.X - circle.Radius;
            double right  = circle.CenterPoint.X + circle.Radius;
            double bottom = circle.CenterPoint.Y - circle.Radius;
            double top    = circle.CenterPoint.Y + circle.Radius;

            return left >= minX && right <= maxX && bottom >= minY && top <= maxY;
        }

        // Arc entity (inherits CadCircle)
        if (obj is CadArc arc)
        {
            // Conservative check using full circle bounds
            double left   = arc.CenterPoint.X - arc.Radius;
            double right  = arc.CenterPoint.X + arc.Radius;
            double bottom = arc.CenterPoint.Y - arc.Radius;
            double top    = arc.CenterPoint.Y + arc.Radius;

            return left >= minX && right <= maxX && bottom >= minY && top <= maxY;
        }

        // 2‑D lightweight polyline
        if (obj is CadLwPolyline lwPoly)
        {
            foreach (Cad2DPoint pt in lwPoly.Coordinates)
            {
                if (!PointInside(pt, minX, minY, maxX, maxY))
                    return false;
            }
            return true;
        }

        // 3‑D polyline
        if (obj is CadPolyline poly3d)
        {
            foreach (CadVertexBase vertex in poly3d.ChildObjects.OfType<CadVertexBase>())
            {
                var pt3d = vertex.LocationPoint;
                if (!PointInside(new Cad2DPoint(pt3d.X, pt3d.Y), minX, minY, maxX, maxY))
                    return false;
            }
            return true;
        }

        // Other entity types are not removed
        return false;
    }

    // Checks whether a 2‑D point lies within the rectangle (inclusive)
    static bool PointInside(Cad2DPoint pt, double minX, double minY, double maxX, double maxY)
    {
        return pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y <= maxY;
    }
}