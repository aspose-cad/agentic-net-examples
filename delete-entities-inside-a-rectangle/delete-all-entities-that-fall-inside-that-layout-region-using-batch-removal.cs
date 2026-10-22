using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

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

                // Define the layout region (example rectangle)
                double regionMinX = 0.0;
                double regionMinY = 0.0;
                double regionMaxX = 100.0;
                double regionMaxY = 100.0;

                // Collect entities that lie completely inside the region
                List<CadEntityBase> entitiesToRemove = new List<CadEntityBase>();

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (IsEntityInsideRegion(entity, regionMinX, regionMinY, regionMaxX, regionMaxY))
                    {
                        entitiesToRemove.Add(entity);
                    }
                }

                // Remove the collected entities by creating a new collection without them
                CadEntityBase[] filtered = cadImage.Entities.Except(entitiesToRemove).ToArray();
                cadImage.Entities = filtered;

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static bool IsEntityInsideRegion(CadEntityBase entity, double minX, double minY, double maxX, double maxY)
    {
        // CadLine: both endpoints must be inside the region
        if (entity is CadLine line)
        {
            return IsPointInside(line.FirstPoint, minX, minY, maxX, maxY) &&
                   IsPointInside(line.SecondPoint, minX, minY, maxX, maxY);
        }

        // CadLwPolyline: all vertices must be inside the region
        if (entity is CadLwPolyline polyline)
        {
            foreach (Cad2DPoint pt in polyline.Coordinates)
            {
                if (!IsPointInside(pt, minX, minY, maxX, maxY))
                    return false;
            }
            return true;
        }

        // Cad3DFace: all four corners must be inside the region
        if (entity is Cad3DFace face)
        {
            return IsPointInside(face.FirstCorner, minX, minY, maxX, maxY) &&
                   IsPointInside(face.SecondCorner, minX, minY, maxX, maxY) &&
                   IsPointInside(face.ThirdCorner, minX, minY, maxX, maxY) &&
                   IsPointInside(face.FourthCorner, minX, minY, maxX, maxY);
        }

        // CadInsertObject: treat as outside (or implement recursive check if needed)
        return false;
    }

    static bool IsPointInside(Cad3DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }

    static bool IsPointInside(Cad2DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }
}