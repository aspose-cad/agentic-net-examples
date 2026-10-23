using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dwg";
        string outputPath = "output.dxf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
            {
                // Create a closed 2D polyline (rectangle)
                var rectangle = new CadLwPolyline
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

                // Add the polyline to Model Space
                CadBlockEntity modelSpace = cadImage.BlockEntities["*MODEL_SPACE"];
                modelSpace.AddEntity(rectangle);

                // Create a solid hatch that uses the rectangle as its boundary
                var hatch = new CadHatch
                {
                    PatternName = "SOLID",
                    HatchAngle = 0.0,
                    HatchScaleOrSpacing = 1.0,
                    AssociativeFlag = (short)1,
                    SolidFillFlag = (short)1,
                    ColorId = (short)7 // white (indexed color)
                };

                // Define the boundary path container (polyline boundary)
                var boundaryContainer = new CadHatchBoundaryPathContainer
                {
                    PathType = 2, // polyline boundary
                    SourceBoundaryObjects = new List<string>()
                };

                var polyBoundary = new CadPolylineBoundaryPath
                {
                    IsClosed = true,
                    Vertices = new List<Point2D>
                    {
                        new Point2D(0, 0),
                        new Point2D(100, 0),
                        new Point2D(100, 50),
                        new Point2D(0, 50)
                    }
                };
                polyBoundary.MaxArrayLen = polyBoundary.Vertices.Count;

                boundaryContainer.BoundaryPath = new List<ICadBoundaryPath> { polyBoundary };

                hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { boundaryContainer };

                // Add the hatch after the polyline to ensure proper layering
                modelSpace.AddEntity(hatch);

                // Save the modified CAD drawing as DXF (ASCII format)
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };
                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}