using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

namespace CadRectangleExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dxf";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD object model
                        CadImage cadImage = (CadImage)image;

                        // -------------------------------------------------
                        // Create a rectangle as a 2D lightweight polyline
                        // -------------------------------------------------
                        CadLwPolyline rectangle = new CadLwPolyline();

                        // Define rectangle corner points (counter‑clockwise)
                        List<Cad2DPoint> rectPoints = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(100, 0),
                            new Cad2DPoint(100, 50),
                            new Cad2DPoint(0, 50)
                        };

                        // Assign coordinates and close the polyline
                        rectangle.Coordinates.AddRange(rectPoints);
                        rectangle.Flag = CadLwPolylineFlag.Closed;

                        // -------------------------------------------------
                        // Remove entities that are completely inside the rectangle
                        // (simple example: remove all 3D faces whose all corners lie inside)
                        // -------------------------------------------------
                        IEnumerable<CadEntityBase> filteredEntities = cadImage.Entities.Where(entity =>
                        {
                            // Keep the entity unless it is a 3D face fully inside the rectangle
                            if (entity is Cad3DFace face)
                            {
                                // Helper to check if a point is inside the rectangle bounds
                                bool IsInside(Cad3DPoint pt) =>
                                    pt.X >= 0 && pt.X <= 100 && pt.Y >= 0 && pt.Y <= 50;

                                return !(IsInside(face.FirstCorner) &&
                                         IsInside(face.SecondCorner) &&
                                         IsInside(face.ThirdCorner) &&
                                         IsInside(face.FourthCorner));
                            }

                            // Keep all other entity types
                            return true;
                        });

                        // -------------------------------------------------
                        // Append the new rectangle to the drawing entities
                        // -------------------------------------------------
                        CadEntityBase[] newEntityArray = filteredEntities
                            .Concat(new CadEntityBase[] { rectangle })
                            .ToArray();

                        // Replace the Entities collection with the new array
                        cadImage.Entities = newEntityArray;

                        // -------------------------------------------------
                        // Save the modified drawing as DXF
                        // -------------------------------------------------
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
    }
}