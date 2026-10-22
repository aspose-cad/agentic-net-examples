using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path for the resulting CAD file
                string outputPath = "output.dwg";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Define rectangle bounds for deletion (example values)
                double minX = 0.0;
                double minY = 0.0;
                double maxX = 100.0;
                double maxY = 100.0;

                // Load the CAD document
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Build a new collection of entities that should be kept
                    List<CadEntityBase> keptEntities = new List<CadEntityBase>();

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        bool delete = false;

                        // Handle CadLine entities
                        if (entity is CadLine line)
                        {
                            bool firstInside = line.FirstPoint.X >= minX && line.FirstPoint.X <= maxX &&
                                               line.FirstPoint.Y >= minY && line.FirstPoint.Y <= maxY;
                            bool secondInside = line.SecondPoint.X >= minX && line.SecondPoint.X <= maxX &&
                                                line.SecondPoint.Y >= minY && line.SecondPoint.Y <= maxY;

                            // Delete the line if both endpoints lie within the rectangle
                            if (firstInside && secondInside)
                                delete = true;
                        }
                        // Handle CadLwPolyline entities
                        else if (entity is CadLwPolyline polyline)
                        {
                            bool allInside = true;
                            foreach (var point in polyline.Coordinates)
                            {
                                if (point.X < minX || point.X > maxX || point.Y < minY || point.Y > maxY)
                                {
                                    allInside = false;
                                    break;
                                }
                            }

                            if (allInside)
                                delete = true;
                        }
                        // Additional entity types (e.g., CadCircle, CadArc, Cad3DFace) can be processed similarly
                        // Example for CadCircle
                        else if (entity is CadCircle circle)
                        {
                            bool centerInside = circle.CenterPoint.X >= minX && circle.CenterPoint.X <= maxX &&
                                                circle.CenterPoint.Y >= minY && circle.CenterPoint.Y <= maxY;
                            // Simple heuristic: delete if center is inside and radius fits within rectangle
                            if (centerInside &&
                                circle.CenterPoint.X - circle.Radius >= minX && circle.CenterPoint.X + circle.Radius <= maxX &&
                                circle.CenterPoint.Y - circle.Radius >= minY && circle.CenterPoint.Y + circle.Radius <= maxY)
                            {
                                delete = true;
                            }
                        }

                        if (!delete)
                            keptEntities.Add(entity);
                    }

                    // Replace the entity collection with the filtered list
                    cadImage.Entities = keptEntities.ToArray();

                    // Save the modified CAD document
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}