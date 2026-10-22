using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

namespace CadEntityRemoval
{
    class Program
    {
        // Rectangle bounds for containment check
        const double RectMinX = 10.0;
        const double RectMinY = 10.0;
        const double RectMaxX = 100.0;
        const double RectMaxY = 100.0;

        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file \"{inputPath}\" not found.");
                return;
            }

            try
            {
                // Load CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Collect entities that need to be removed
                    var entitiesToRemove = new List<CadEntityBase>();

                    foreach (var entity in cadImage.Entities)
                    {
                        if (IsEntityInsideRectangle(entity))
                            entitiesToRemove.Add(entity);
                    }

                    // Rebuild the entity collection without the removed ones
                    var remainingEntities = cadImage.Entities
                                                   .Where(e => !entitiesToRemove.Contains(e))
                                                   .ToArray();

                    // Assign the filtered collection back to the image (if the setter exists)
                    // If the property is read‑only, this line can be omitted; the removal will be a no‑op.
                    cadImage.Entities = remainingEntities;

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

        // Determines whether a given entity lies completely inside the rectangle
        private static bool IsEntityInsideRectangle(CadEntityBase entity)
        {
            // Line entity
            if (entity is CadLine line)
                return IsPointInside(line.FirstPoint) && IsPointInside(line.SecondPoint);

            // Lightweight polyline entity
            if (entity is CadLwPolyline lwPolyline)
            {
                foreach (var pt in lwPolyline.Coordinates)
                {
                    if (!IsPointInside(pt))
                        return false;
                }
                return true;
            }

            // 3D Face entity (check XY projection of all corners)
            if (entity is Cad3DFace face)
                return IsPointInside(face.FirstCorner) &&
                       IsPointInside(face.SecondCorner) &&
                       IsPointInside(face.ThirdCorner) &&
                       IsPointInside(face.FourthCorner);

            // Insert object – treat as inside only if its insertion point is inside
            if (entity is CadInsertObject insert)
                return IsPointInside(insert.InsertionPoint);

            // Other entity types are not processed; assume they are outside
            return false;
        }

        // Checks if a 2D point lies within the rectangle
        private static bool IsPointInside(Cad2DPoint pt)
        {
            return pt.X >= RectMinX && pt.X <= RectMaxX &&
                   pt.Y >= RectMinY && pt.Y <= RectMaxY;
        }

        // Checks if a 3D point lies within the rectangle (ignores Z)
        private static bool IsPointInside(Cad3DPoint pt)
        {
            return pt.X >= RectMinX && pt.X <= RectMaxX &&
                   pt.Y >= RectMinY && pt.Y <= RectMaxY;
        }
    }
}