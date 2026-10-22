using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

namespace HatchRectangleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "template.dxf";
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "output.dxf");

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the existing DXF file
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Get the first layout record
                CadLayout layout = cadImage.Layouts.Values.FirstOrDefault();
                if (layout == null)
                {
                    Console.WriteLine("No layout found in the CAD file.");
                    return;
                }

                // Create a new hatch entity
                CadHatch hatch = new CadHatch
                {
                    PatternName = "MyCustomPattern",
                    HatchAngle = 0.0,
                    HatchScaleOrSpacing = 1.0,
                    ColorId = 1,               // Indexed color (e.g., red)
                    SolidFillFlag = 0          // 0 = not solid fill
                };

                // Define a simple line pattern for the hatch
                CadHatchPatternData patternData = new CadHatchPatternData
                {
                    LineAngle = 45.0,
                    LineBasePoint = new Cad2DPoint(0, 0),
                    LineOffset = new Cad2DPoint(0.5, 0),
                    DashLengths = new List<double>()
                };

                hatch.PatternDefinitions = new List<CadHatchPatternData> { patternData };

                // Create boundary path container (polyline rectangle)
                CadHatchBoundaryPathContainer boundaryContainer = new CadHatchBoundaryPathContainer
                {
                    PathType = 2, // Polyline boundary
                    BoundaryPath = new List<ICadBoundaryPath>(),
                    SourceBoundaryObjects = new List<string>()
                };

                // Define the polyline boundary (rectangle)
                CadPolylineBoundaryPath polylineBoundary = new CadPolylineBoundaryPath
                {
                    Vertices = new List<Point2D>
                    {
                        new Point2D(0, 0),
                        new Point2D(100, 0),
                        new Point2D(100, 50),
                        new Point2D(0, 50),
                        new Point2D(0, 0) // close the polyline
                    },
                    IsClosed = true,
                    Bugles = new List<double>(),
                    HasBugle = false
                };
                polylineBoundary.MaxArrayLen = polylineBoundary.Vertices.Count;

                // Add the polyline to the container
                boundaryContainer.BoundaryPath.Add(polylineBoundary);

                // Assign boundary paths to the hatch
                hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { boundaryContainer };

                // Set the owner of the hatch to the layout's block table record
                hatch.SoftOwner = layout.BlockTableRecordHandle;

                // Add the hatch to the drawing's entities
                cadImage.Entities = cadImage.Entities.Concat(new[] { hatch }).ToArray();

                // Ensure output directory exists
                Directory.CreateDirectory(outputDir);

                // Save the modified drawing as DXF (ASCII format)
                DxfOptions dxfOptions = new DxfOptions
                {
                    DxfFileFormat = CadFileFormat.Ascii,
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);

                Console.WriteLine($"Hatched rectangle saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}