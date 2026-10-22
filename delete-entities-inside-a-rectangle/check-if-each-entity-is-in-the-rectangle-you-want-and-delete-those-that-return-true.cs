using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

namespace CadEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
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
                // Define the rectangle bounds
                double rectMinX = 0.0;
                double rectMinY = 0.0;
                double rectMaxX = 100.0;
                double rectMaxY = 100.0;

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // List to hold entities that need to be removed
                    List<CadEntityBase> entitiesToRemove = new List<CadEntityBase>();

                    // Iterate over top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        bool shouldDelete = false;

                        // Check CadLine
                        if (entity is CadLine line)
                        {
                            if (IsPointInsideRectangle(line.FirstPoint, rectMinX, rectMinY, rectMaxX, rectMaxY) ||
                                IsPointInsideRectangle(line.SecondPoint, rectMinX, rectMinY, rectMaxX, rectMaxY))
                            {
                                shouldDelete = true;
                            }
                        }
                        // Check CadLwPolyline
                        else if (entity is CadLwPolyline polyline)
                        {
                            foreach (Cad2DPoint pt in polyline.Coordinates)
                            {
                                if (IsPointInsideRectangle(pt, rectMinX, rectMinY, rectMaxX, rectMaxY))
                                {
                                    shouldDelete = true;
                                    break;
                                }
                            }
                        }
                        // Check Cad3DFace
                        else if (entity is Cad3DFace face)
                        {
                            if (IsPointInsideRectangle(face.FirstCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) ||
                                IsPointInsideRectangle(face.SecondCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) ||
                                IsPointInsideRectangle(face.ThirdCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) ||
                                IsPointInsideRectangle(face.FourthCorner, rectMinX, rectMinY, rectMaxX, rectMaxY))
                            {
                                shouldDelete = true;
                            }
                        }
                        // Check CadPolyline (3D)
                        else if (entity is CadPolyline cadPolyline)
                        {
                            foreach (var vertex in cadPolyline.ChildObjects.OfType<CadVertexBase>())
                            {
                                if (IsPointInsideRectangle(vertex.LocationPoint, rectMinX, rectMinY, rectMaxX, rectMaxY))
                                {
                                    shouldDelete = true;
                                    break;
                                }
                            }
                        }

                        if (shouldDelete)
                        {
                            entitiesToRemove.Add(entity);
                        }
                    }

                    // Build a new collection without the entities to remove
                    CadEntityBase[] newEntities = cadImage.Entities
                        .Where(e => !entitiesToRemove.Contains(e))
                        .ToArray();

                    // Replace the Entities collection
                    cadImage.Entities = newEntities;

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

        // Helper for Cad3DPoint
        private static bool IsPointInsideRectangle(Cad3DPoint point, double minX, double minY, double maxX, double maxY)
        {
            return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
        }

        // Helper for Cad2DPoint
        private static bool IsPointInsideRectangle(Cad2DPoint point, double minX, double minY, double maxX, double maxY)
        {
            return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
        }
    }
}