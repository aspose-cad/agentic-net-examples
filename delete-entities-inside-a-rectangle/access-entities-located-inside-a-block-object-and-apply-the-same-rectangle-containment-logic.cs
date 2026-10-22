using System;
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
        // Define rectangle bounds
        double minX = 0.0;
        double minY = 0.0;
        double maxX = 100.0;
        double maxY = 100.0;

        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            // Load CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImg = (CadImage)cadImage;

                // Iterate over block definitions
                foreach (CadBlockEntity block in cadImg.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Inspecting block: {block.Name}");

                    // Iterate over entities inside the block
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        bool isInside = false;

                        // Apply rectangle containment based on entity type
                        if (entity is CadLine line)
                        {
                            isInside = IsPointInside(line.FirstPoint, minX, minY, maxX, maxY) &&
                                       IsPointInside(line.SecondPoint, minX, minY, maxX, maxY);
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            isInside = true;
                            foreach (Cad2DPoint pt in polyline.Coordinates)
                            {
                                if (!IsPointInside(pt, minX, minY, maxX, maxY))
                                {
                                    isInside = false;
                                    break;
                                }
                            }
                        }
                        else if (entity is CadPolyline poly3d)
                        {
                            // 3D polyline: check each vertex point
                            isInside = true;
                            var vertices = poly3d.ChildObjects
                                .OfType<CadVertexBase>()
                                .Select(v => v.LocationPoint);
                            foreach (var pt in vertices)
                            {
                                if (!IsPointInside(pt, minX, minY, maxX, maxY))
                                {
                                    isInside = false;
                                    break;
                                }
                            }
                        }
                        else if (entity is Cad3DFace face)
                        {
                            isInside = IsPointInside(face.FirstCorner, minX, minY, maxX, maxY) &&
                                       IsPointInside(face.SecondCorner, minX, minY, maxX, maxY) &&
                                       IsPointInside(face.ThirdCorner, minX, minY, maxX, maxY) &&
                                       IsPointInside(face.FourthCorner, minX, minY, maxX, maxY);
                        }
                        else if (entity is CadInsertObject insert)
                        {
                            // Log insert object name; further inspection can be added if needed
                            Console.WriteLine($"Insert object: {insert.Name}");
                        }

                        if (isInside)
                        {
                            Console.WriteLine($"Entity {entity.TypeName} is inside the rectangle.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Helper for 3D points
    static bool IsPointInside(Cad3DPoint pt, double minX, double minY, double maxX, double maxY)
    {
        return pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y <= maxY;
    }

    // Helper for 2D points
    static bool IsPointInside(Cad2DPoint pt, double minX, double minY, double maxX, double maxY)
    {
        return pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y <= maxY;
    }
}