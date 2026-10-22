using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockEntityDeletion
{
    class Program
    {
        struct Rectangle2D
        {
            public double MinX;
            public double MinY;
            public double MaxX;
            public double MaxY;

            public Rectangle2D(double minX, double minY, double maxX, double maxY)
            {
                MinX = minX;
                MinY = minY;
                MaxX = maxX;
                MaxY = maxY;
            }

            public bool Intersects(double x1, double y1, double x2, double y2)
            {
                double rectMinX = Math.Min(x1, x2);
                double rectMaxX = Math.Max(x1, x2);
                double rectMinY = Math.Min(y1, y2);
                double rectMaxY = Math.Max(y1, y2);

                return !(rectMaxX < MinX || rectMinX > MaxX || rectMaxY < MinY || rectMinY > MaxY);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                string sourcePath = "input.dwg";
                string outputPath = "output.dwg";

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = (CadImage)image;

                    Rectangle2D selectionRect = new Rectangle2D(10.0, 10.0, 200.0, 200.0);

                    // Process top‑level entities (collect then recreate list without removed items)
                    var topEntities = new List<CadEntityBase>(cadImage.Entities);
                    DeleteIntersectingEntities(topEntities, selectionRect);
                    // Note: CadImage.Entities is read‑only; in a real scenario you would need API support to replace them.
                    // For demonstration we only output removed entities.

                    // Process block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        var blockEntities = new List<CadEntityBase>(block.Entities);
                        DeleteIntersectingEntities(blockEntities, selectionRect);
                        // Clear existing and add back the filtered entities
                        block.Entities.Clear();
                        foreach (var ent in blockEntities)
                        {
                            block.AddEntity(ent);
                        }
                    }

                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        private static void DeleteIntersectingEntities(IList<CadEntityBase> entities, Rectangle2D rect)
        {
            List<CadEntityBase> toRemove = new List<CadEntityBase>();

            foreach (CadEntityBase entity in entities)
            {
                if (EntityIntersectsRectangle(entity, rect))
                {
                    toRemove.Add(entity);
                }
            }

            foreach (CadEntityBase entity in toRemove)
            {
                entities.Remove(entity);
                Console.WriteLine($"Removed entity of type {entity.GetType().Name}");
            }
        }

        private static bool EntityIntersectsRectangle(CadEntityBase entity, Rectangle2D rect)
        {
            if (entity is CadLine line)
            {
                double x1 = line.FirstPoint.X;
                double y1 = line.FirstPoint.Y;
                double x2 = line.SecondPoint.X;
                double y2 = line.SecondPoint.Y;
                return rect.Intersects(x1, y1, x2, y2);
            }

            if (entity is CadLwPolyline lwPolyline)
            {
                IList<Cad2DPoint> points = lwPolyline.Coordinates;
                for (int i = 0; i < points.Count - 1; i++)
                {
                    double x1 = points[i].X;
                    double y1 = points[i].Y;
                    double x2 = points[i + 1].X;
                    double y2 = points[i + 1].Y;
                    if (rect.Intersects(x1, y1, x2, y2))
                        return true;
                }

                // Close polyline if flagged as closed
                if ((lwPolyline.Flag & CadLwPolylineFlag.Closed) != 0 && points.Count > 1)
                {
                    double x1 = points[points.Count - 1].X;
                    double y1 = points[points.Count - 1].Y;
                    double x2 = points[0].X;
                    double y2 = points[0].Y;
                    if (rect.Intersects(x1, y1, x2, y2))
                        return true;
                }

                return false;
            }

            if (entity is Cad3DFace face)
            {
                Cad3DPoint[] corners = new Cad3DPoint[]
                {
                    face.FirstCorner,
                    face.SecondCorner,
                    face.ThirdCorner,
                    face.FourthCorner
                };

                for (int i = 0; i < corners.Length; i++)
                {
                    Cad3DPoint p1 = corners[i];
                    Cad3DPoint p2 = corners[(i + 1) % corners.Length];
                    if (rect.Intersects(p1.X, p1.Y, p2.X, p2.Y))
                        return true;
                }

                return false;
            }

            if (entity is CadInsertObject insert)
            {
                if (insert.InsertionPoint != null)
                {
                    double x = insert.InsertionPoint.X;
                    double y = insert.InsertionPoint.Y;
                    return x >= rect.MinX && x <= rect.MaxX && y >= rect.MinY && y <= rect.MaxY;
                }
                return false;
            }

            return false;
        }
    }
}