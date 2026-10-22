using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dxf";

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
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Define rectangle bounds (example coordinates)
                    double minX = 0.0;
                    double minY = 0.0;
                    double maxX = 100.0;
                    double maxY = 100.0;

                    // Create a rectangle polyline (closed LWPolyline) – optional, not added to drawing
                    CadLwPolyline rectangle = new CadLwPolyline();
                    rectangle.Coordinates.Add(new Cad2DPoint(minX, minY));
                    rectangle.Coordinates.Add(new Cad2DPoint(maxX, minY));
                    rectangle.Coordinates.Add(new Cad2DPoint(maxX, maxY));
                    rectangle.Coordinates.Add(new Cad2DPoint(minX, maxY));
                    rectangle.Flag = CadLwPolylineFlag.Closed;

                    // Filter out entities that lie completely inside the rectangle
                    CadEntityBase[] filteredEntities = cadImage.Entities
                        .Where(e => !IsEntityInsideRectangle(e, minX, maxX, minY, maxY))
                        .ToArray();

                    // Replace the drawing's entities with the filtered collection
                    cadImage.Entities = filteredEntities;

                    // Save the modified drawing
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        // Determines whether a given entity is inside the defined rectangle.
        private static bool IsEntityInsideRectangle(CadEntityBase entity, double minX, double maxX, double minY, double maxY)
        {
            // Handle LWPolyline entities
            if (entity is CadLwPolyline polyline && polyline.Coordinates.Count > 0)
            {
                // Simple check: if all points are inside the rectangle, consider the entity inside
                foreach (Cad2DPoint pt in polyline.Coordinates)
                {
                    if (pt.X < minX || pt.X > maxX || pt.Y < minY || pt.Y > maxY)
                        return false;
                }
                return true;
            }

            // Handle 3D Polyline entities
            if (entity is CadPolyline poly3d)
            {
                var vertices = poly3d.ChildObjects.OfType<CadVertexBase>();
                foreach (CadVertexBase vertex in vertices)
                {
                    Cad3DPoint pt = vertex.LocationPoint;
                    if (pt.X < minX || pt.X > maxX || pt.Y < minY || pt.Y > maxY)
                        return false;
                }
                return true;
            }

            // Handle 3D Face entities
            if (entity is Cad3DFace face)
            {
                Cad3DPoint[] corners = new[] { face.FirstCorner, face.SecondCorner, face.ThirdCorner, face.FourthCorner };
                foreach (Cad3DPoint pt in corners)
                {
                    if (pt.X < minX || pt.X > maxX || pt.Y < minY || pt.Y > maxY)
                        return false;
                }
                return true;
            }

            // Other entity types are not considered for deletion
            return false;
        }
    }
}