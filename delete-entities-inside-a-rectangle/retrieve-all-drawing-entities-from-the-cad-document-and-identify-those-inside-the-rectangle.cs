using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Rectangle bounds
        double rectMinX = 0.0;
        double rectMinY = 0.0;
        double rectMaxX = 100.0;
        double rectMaxY = 100.0;

        const string inputPath = "input.dwg"; // placeholder path

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            // Load CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Iterate top-level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    bool isInside = false;

                    if (entity is CadLine line)
                    {
                        // Both line endpoints must be inside the rectangle
                        isInside = IsPointInside(line.FirstPoint, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                   IsPointInside(line.SecondPoint, rectMinX, rectMinY, rectMaxX, rectMaxY);
                    }
                    else if (entity is CadLwPolyline polyline)
                    {
                        // All polyline vertices must be inside the rectangle
                        isInside = true;
                        foreach (Cad2DPoint pt in polyline.Coordinates)
                        {
                            if (!IsPointInside(pt, rectMinX, rectMinY, rectMaxX, rectMaxY))
                            {
                                isInside = false;
                                break;
                            }
                        }
                    }
                    else if (entity is Cad3DFace face)
                    {
                        // All face corners must be inside the rectangle (projected to XY)
                        isInside = IsPointInside(face.FirstCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                   IsPointInside(face.SecondCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                   IsPointInside(face.ThirdCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                   IsPointInside(face.FourthCorner, rectMinX, rectMinY, rectMaxX, rectMaxY);
                    }

                    if (isInside)
                    {
                        Console.WriteLine($"Entity Type: {entity.GetType().Name} is inside the rectangle.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
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