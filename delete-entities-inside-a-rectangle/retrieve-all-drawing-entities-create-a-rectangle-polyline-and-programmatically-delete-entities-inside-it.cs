using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CadEntityManipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWG/DXF file
                string sourcePath = "input.dwg";
                // Path to the output file
                string outputPath = "output.dxf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Retrieve all top‑level drawing entities
                    IEnumerable<CadEntityBase> allEntities = cadImage.Entities;

                    // Create a rectangle polyline (closed LWPolyline)
                    CadLwPolyline rectangle = new CadLwPolyline
                    {
                        Flag = CadLwPolylineFlag.Closed,
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(100, 0),
                            new Cad2DPoint(100, 50),
                            new Cad2DPoint(0, 50)
                        }
                    };
                    rectangle.PointCount = rectangle.Coordinates.Count;

                    // Determine rectangle bounds for containment test
                    double minX = rectangle.Coordinates.Min(p => p.X);
                    double maxX = rectangle.Coordinates.Max(p => p.X);
                    double minY = rectangle.Coordinates.Min(p => p.Y);
                    double maxY = rectangle.Coordinates.Max(p => p.Y);

                    // Filter out entities that lie completely inside the rectangle
                    IEnumerable<CadEntityBase> filteredEntities = allEntities
                        .Where(e => !IsEntityInsideRectangle(e, minX, maxX, minY, maxY));

                    // Append the rectangle to the drawing
                    CadEntityBase[] newEntityArray = filteredEntities
                        .Concat(new CadEntityBase[] { rectangle })
                        .ToArray();

                    // Replace the drawing's entity collection
                    cadImage.Entities = newEntityArray;

                    // Save the modified drawing as DXF
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, dxfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        // Helper method to determine if an entity is completely inside the rectangle bounds
        private static bool IsEntityInsideRectangle(CadEntityBase entity, double minX, double maxX, double minY, double maxY)
        {
            // Handle LWPolyline entities
            if (entity is CadLwPolyline lwPolyline)
            {
                foreach (Cad2DPoint pt in lwPolyline.Coordinates)
                {
                    if (pt.X < minX || pt.X > maxX || pt.Y < minY || pt.Y > maxY)
                        return false;
                }
                return true;
            }

            // Handle 3D Polyline entities
            if (entity is CadPolyline polyline)
            {
                foreach (var vertex in polyline.ChildObjects.OfType<CadVertexBase>())
                {
                    Cad3DPoint pt = vertex.LocationPoint;
                    if (pt.X < minX || pt.X > maxX || pt.Y < minY || pt.Y > maxY)
                        return false;
                }
                return true;
            }

            // For other entity types, assume they are not inside the rectangle
            return false;
        }
    }
}