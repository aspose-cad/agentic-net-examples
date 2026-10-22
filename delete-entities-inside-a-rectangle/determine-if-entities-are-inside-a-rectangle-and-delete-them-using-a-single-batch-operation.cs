using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Define rectangle bounds (example values)
                double rectMinX = 0.0;
                double rectMinY = 0.0;
                double rectMaxX = 100.0;
                double rectMaxY = 100.0;

                // Collect entities that lie completely inside the rectangle
                List<CadEntityBase> entitiesToDelete = new List<CadEntityBase>();

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (IsEntityInsideRectangle(entity, rectMinX, rectMinY, rectMaxX, rectMaxY))
                    {
                        entitiesToDelete.Add(entity);
                    }
                }

                // Perform batch deletion by creating a new entity collection without the unwanted ones
                CadEntityBase[] filteredEntities = cadImage.Entities
                    .Where(e => !entitiesToDelete.Contains(e))
                    .ToArray();

                // Replace the entities collection
                cadImage.Entities = filteredEntities;

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }

    // Determines whether a CAD entity is fully inside the specified rectangle
    static bool IsEntityInsideRectangle(CadEntityBase entity, double minX, double minY, double maxX, double maxY)
    {
        switch (entity)
        {
            case CadLine line:
                return PointInside(line.FirstPoint, minX, minY, maxX, maxY) &&
                       PointInside(line.SecondPoint, minX, minY, maxX, maxY);

            case CadLwPolyline polyline:
                foreach (Cad2DPoint pt in polyline.Coordinates)
                {
                    if (!PointInside(pt, minX, minY, maxX, maxY))
                        return false;
                }
                return true;

            case Cad3DFace face:
                return PointInside(face.FirstCorner, minX, minY, maxX, maxY) &&
                       PointInside(face.SecondCorner, minX, minY, maxX, maxY) &&
                       PointInside(face.ThirdCorner, minX, minY, maxX, maxY) &&
                       PointInside(face.FourthCorner, minX, minY, maxX, maxY);

            case CadPolyline polyline3D:
                foreach (var vertex in polyline3D.ChildObjects.OfType<CadVertexBase>())
                {
                    if (!PointInside(vertex.LocationPoint, minX, minY, maxX, maxY))
                        return false;
                }
                return true;

            default:
                // Other entity types are ignored for this operation
                return false;
        }
    }

    // Checks if a 3D point lies within the rectangle (Z is ignored)
    static bool PointInside(Cad3DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }

    // Checks if a 2D point lies within the rectangle
    static bool PointInside(Cad2DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }
}