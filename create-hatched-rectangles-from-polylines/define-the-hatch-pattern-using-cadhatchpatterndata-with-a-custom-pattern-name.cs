using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.Primitives;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AsposeCadExample
{
    class Program
    {
        static void Main()
        {
            string templatePath = "template.dxf";
            string outputPath = "HatchedRectangle.dxf";

            if (!File.Exists(templatePath))
            {
                Console.WriteLine($"Template file not found: {templatePath}");
                return;
            }

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(templatePath))
                {
                    var block = new CadBlockEntity();

                    var p1 = new Cad2DPoint(0, 0);
                    var p2 = new Cad2DPoint(100, 0);
                    var p3 = new Cad2DPoint(100, 50);
                    var p4 = new Cad2DPoint(0, 50);

                    var rectangle = new CadLwPolyline();
                    rectangle.Coordinates.Add(p1);
                    rectangle.Coordinates.Add(p2);
                    rectangle.Coordinates.Add(p3);
                    rectangle.Coordinates.Add(p4);
                    rectangle.Flag = CadLwPolylineFlag.Closed;

                    block.AddEntity(rectangle);

                    var hatch = new CadHatch
                    {
                        PatternName = "MyCustomPattern",
                        HatchAngle = 0.0,
                        HatchScaleOrSpacing = 1.0,
                        AssociativeFlag = 0,
                        SolidFillFlag = 0,
                        ColorId = 7 // White (indexed color)
                    };

                    // Define a simple pattern definition (line family)
                    var patternDef = new CadHatchPatternData();
                    hatch.PatternDefinitions = new List<CadHatchPatternData> { patternDef };

                    var boundaryContainer = new CadHatchBoundaryPathContainer
                    {
                        PathType = 2,
                        BoundaryPath = new List<ICadBoundaryPath>()
                    };

                    var polylineBoundary = new CadPolylineBoundaryPath
                    {
                        IsClosed = true,
                        Vertices = new List<Point2D>(),
                        Bugles = new List<double>(),
                        HasBugle = false
                    };

                    polylineBoundary.Vertices.Add(new Point2D(p1.X, p1.Y));
                    polylineBoundary.Vertices.Add(new Point2D(p2.X, p2.Y));
                    polylineBoundary.Vertices.Add(new Point2D(p3.X, p3.Y));
                    polylineBoundary.Vertices.Add(new Point2D(p4.X, p4.Y));
                    polylineBoundary.MaxArrayLen = polylineBoundary.Vertices.Count;

                    boundaryContainer.BoundaryPath.Add(polylineBoundary);
                    boundaryContainer.BoundaryObjectCount = boundaryContainer.BoundaryPath.Count;
                    boundaryContainer.SourceBoundaryObjects = new List<string>();

                    hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { boundaryContainer };

                    block.AddEntity(hatch);

                    string blockName = "HatchedRectBlock";
                    cadImage.BlockEntities.Add(blockName, block);

                    var dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Ascii
                    };

                    cadImage.Save(outputPath, dxfOptions);
                    Console.WriteLine($"File saved successfully to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}