using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

namespace CadEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths (replace with actual paths)
                string inputPath = "input.dwg";
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                string outputPath = "output.dwg";

                // Define rectangle region (layout) coordinates
                double rectMinX = 0.0;
                double rectMinY = 0.0;
                double rectMaxX = 100.0;
                double rectMaxY = 100.0;

                // Load CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Iterate over all blocks (including model space) and remove entities inside the rectangle
                    foreach (var block in cadImage.BlockEntities.ValuesTyped)
                    {
                        var entitiesToRemove = new List<CadEntityBase>();

                        foreach (var entity in block.Entities)
                        {
                            if (EntityIsInsideRectangle(entity, rectMinX, rectMinY, rectMaxX, rectMaxY))
                                entitiesToRemove.Add(entity);
                        }

                        foreach (var entity in entitiesToRemove)
                        {
                            // Remove from the block's entity collection
                            block.Entities.Remove(entity);
                        }
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                    Console.WriteLine($"Entities inside the rectangle have been removed. Saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }

        // Determines whether a given entity lies completely inside the rectangle
        private static bool EntityIsInsideRectangle(CadEntityBase entity, double minX, double minY, double maxX, double maxY)
        {
            // Line
            if (entity is CadLine line)
            {
                return PointInside(line.FirstPoint, minX, minY, maxX, maxY) &&
                       PointInside(line.SecondPoint, minX, minY, maxX, maxY);
            }

            // Lightweight polyline
            if (entity is CadLwPolyline lwPolyline)
            {
                foreach (var pt in lwPolyline.Coordinates)
                {
                    if (!PointInside(pt, minX, minY, maxX, maxY))
                        return false;
                }
                return true;
            }

            // 3D Polyline
            if (entity is CadPolyline polyline)
            {
                foreach (var child in polyline.ChildObjects.OfType<CadVertexBase>())
                {
                    if (!PointInside(child.LocationPoint, minX, minY, maxX, maxY))
                        return false;
                }
                return true;
            }

            // 3D Face
            if (entity is Cad3DFace face)
            {
                return PointInside(face.FirstCorner, minX, minY, maxX, maxY) &&
                       PointInside(face.SecondCorner, minX, minY, maxX, maxY) &&
                       PointInside(face.ThirdCorner, minX, minY, maxX, maxY) &&
                       PointInside(face.FourthCorner, minX, minY, maxX, maxY);
            }

            // Insert object – treat as inside if its insertion point is inside
            if (entity is CadInsertObject insert)
            {
                var pt = insert.InsertionPoint;
                return PointInside(pt, minX, minY, maxX, maxY);
            }

            // Other entity types are not processed
            return false;
        }

        // Checks if a 2D point is inside the rectangle
        private static bool PointInside(Cad2DPoint point, double minX, double minY, double maxX, double maxY)
        {
            return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
        }

        // Checks if a 3D point is inside the rectangle (ignores Z)
        private static bool PointInside(Cad3DPoint point, double minX, double minY, double maxX, double maxY)
        {
            return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
        }
    }
}