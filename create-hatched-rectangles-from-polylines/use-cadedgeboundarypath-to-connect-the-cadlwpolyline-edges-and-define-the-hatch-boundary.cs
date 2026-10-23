using Aspose.CAD.FileFormats.Cad.CadConsts;
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

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Locate the Model Space block
                CadBlockEntity modelSpace = null;
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name.Equals("*Model_Space", StringComparison.OrdinalIgnoreCase))
                    {
                        modelSpace = block;
                        break;
                    }
                }

                if (modelSpace == null)
                {
                    Console.WriteLine("Model space block not found.");
                    return;
                }

                // Create a closed lightweight polyline (rectangle)
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

                // Add the rectangle polyline to Model Space
                modelSpace.AddEntity(rectangle);

                // Create hatch with edge boundary
                var hatch = new CadHatch
                {
                    PatternName = "ANSI31",
                    HatchAngle = 0.0,
                    HatchScaleOrSpacing = 1.0,
                    AssociativeFlag = 1,
                    SolidFillFlag = 0,
                    ColorId = 7, // indexed color (white)
                    SeedPoints = new List<Cad2DPoint> { new Cad2DPoint(50, 25) },
                    PatternDefinitions = new List<CadHatchPatternData>()
                };

                // Minimal pattern definition (line family) for ANSI31
                var patternDef = new CadHatchPatternData
                {
                    LineBasePoint = new Cad2DPoint(0, 0),
                    LineOffset = new Cad2DPoint(0, 0),
                    LineAngle = 0.0,
                    DashLengths = new List<double>()
                };
                hatch.PatternDefinitions.Add(patternDef);

                // Define edge boundary path
                var edgePath = new CadEdgeBoundaryPath
                {
                    NumberOfEdges = 4,
                    EdgeTypes = new List<short> { 1, 1, 1, 1 },
                    Objects = new List<ICadBoundaryPathEntity>()
                };

                // Rectangle corner points
                var p1 = new Point2D(0, 0);
                var p2 = new Point2D(100, 0);
                var p3 = new Point2D(100, 50);
                var p4 = new Point2D(0, 50);

                // Create line edges
                var line1 = new CadBoundaryPathLine { FirstPoint = p1, SecondPoint = p2 };
                var line2 = new CadBoundaryPathLine { FirstPoint = p2, SecondPoint = p3 };
                var line3 = new CadBoundaryPathLine { FirstPoint = p3, SecondPoint = p4 };
                var line4 = new CadBoundaryPathLine { FirstPoint = p4, SecondPoint = p1 };

                edgePath.Objects.Add(line1);
                edgePath.Objects.Add(line2);
                edgePath.Objects.Add(line3);
                edgePath.Objects.Add(line4);

                // Container for the edge boundary
                var container = new CadHatchBoundaryPathContainer
                {
                    PathType = 0, // edge boundary
                    BoundaryPath = new List<ICadBoundaryPath> { edgePath },
                    SourceBoundaryObjects = new List<string>()
                };

                // Assign boundary paths to hatch
                hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { container };

                // Add hatch to Model Space
                modelSpace.AddEntity(hatch);

                // Save the modified drawing as DXF
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputPath, dxfOptions);

                Console.WriteLine($"Hatched rectangle saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}