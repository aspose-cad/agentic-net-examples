using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadRectangleHatch
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "empty.dwg";   // placeholder path to an existing CAD file
            const string outputPath = "output.dxf"; // placeholder output path

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // -------------------------------------------------
                    // 1. Create a rectangle represented by a closed LWPolyline.
                    // -------------------------------------------------
                    var rectangle = new CadLwPolyline
                    {
                        Flag = CadLwPolylineFlag.Closed,
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(200, 0),
                            new Cad2DPoint(200, 150),
                            new Cad2DPoint(0, 150)
                        }
                    };
                    rectangle.PointCount = rectangle.Coordinates.Count;

                    // -------------------------------------------------
                    // 2. Create a hatch that fills the rectangle.
                    // -------------------------------------------------
                    // Build the polyline boundary path for the hatch
                    var polylineBoundary = new CadPolylineBoundaryPath
                    {
                        IsClosed = true,
                        Vertices = new List<Point2D>
                        {
                            new Point2D(0, 0),
                            new Point2D(200, 0),
                            new Point2D(200, 150),
                            new Point2D(0, 150)
                        },
                        Bugles = new List<double>(),
                        HasBugle = false
                    };
                    polylineBoundary.MaxArrayLen = polylineBoundary.Vertices.Count;

                    // Container for the boundary path
                    var boundaryContainer = new CadHatchBoundaryPathContainer
                    {
                        PathType = 2, // Polyline boundary
                        BoundaryPath = new List<ICadBoundaryPath> { polylineBoundary },
                        SourceBoundaryObjects = new List<string>()
                    };

                    var hatch = new CadHatch
                    {
                        PatternName = "SOLID",
                        SolidFillFlag = 1, // solid fill
                        BoundaryPaths = new List<CadHatchBoundaryPathContainer> { boundaryContainer }
                    };

                    // Add rectangle and hatch to the drawing
                    cadImage.Entities = cadImage.Entities
                        .Concat(new CadEntityBase[] { rectangle, hatch })
                        .ToArray();

                    // -------------------------------------------------
                    // 3. Remove all entities that lie completely inside the hatch area,
                    //    excluding the rectangle and the hatch itself.
                    // -------------------------------------------------
                    double minX = rectangle.Coordinates.Min(p => p.X);
                    double maxX = rectangle.Coordinates.Max(p => p.X);
                    double minY = rectangle.Coordinates.Min(p => p.Y);
                    double maxY = rectangle.Coordinates.Max(p => p.Y);

                    var filteredEntities = new List<CadEntityBase>();

                    foreach (var entity in cadImage.Entities)
                    {
                        // Preserve the rectangle and the hatch.
                        if (ReferenceEquals(entity, rectangle) || ReferenceEquals(entity, hatch))
                        {
                            filteredEntities.Add(entity);
                            continue;
                        }

                        bool isInside = false;

                        if (entity is CadLwPolyline lwPolyline)
                        {
                            isInside = lwPolyline.Coordinates.All(p =>
                                p.X > minX && p.X < maxX && p.Y > minY && p.Y < maxY);
                        }
                        else if (entity is CadPolyline polyline3D)
                        {
                            var vertices = polyline3D.ChildObjects
                                .OfType<CadVertexBase>()
                                .Select(v => v.LocationPoint);
                            isInside = vertices.All(pt =>
                                pt.X > minX && pt.X < maxX && pt.Y > minY && pt.Y < maxY);
                        }

                        if (!isInside)
                        {
                            filteredEntities.Add(entity);
                        }
                    }

                    cadImage.Entities = filteredEntities.ToArray();

                    // -------------------------------------------------
                    // 4. Save the modified drawing to DXF format.
                    // -------------------------------------------------
                    var saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Binary,
                        Version = DxfOutputVersion.R12
                    };

                    // Ensure the directory for the output file exists.
                    var outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    cadImage.Save(outputPath);
                    Console.WriteLine($"File saved successfully to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}