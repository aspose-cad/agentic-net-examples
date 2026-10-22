using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;

namespace CadHatchExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (adjust as needed)
                string inputPath = "input.dxf";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Helper to create a hatch from a rectangle defined by its corner points
                    CadHatch CreateRectangleHatch(short colorId, double angle, double spacing, List<Point2D> vertices)
                    {
                        // Create polyline boundary path
                        CadPolylineBoundaryPath polyPath = new CadPolylineBoundaryPath
                        {
                            IsClosed = true,
                            Vertices = vertices,
                            MaxArrayLen = vertices.Count
                        };

                        // Create boundary path container
                        CadHatchBoundaryPathContainer container = new CadHatchBoundaryPathContainer
                        {
                            PathType = 2, // Polyline boundary
                            BoundaryPath = new List<ICadBoundaryPath> { polyPath },
                            SourceBoundaryObjects = new List<string>()
                        };

                        // Create hatch
                        CadHatch hatch = new CadHatch
                        {
                            ColorId = colorId,
                            HatchAngle = angle,
                            HatchScaleOrSpacing = spacing,
                            BoundaryPaths = new List<CadHatchBoundaryPathContainer> { container }
                        };

                        return hatch;
                    }

                    // First hatched rectangle (0,0)-(100,0)-(100,50)-(0,50)
                    var rect1Vertices = new List<Point2D>
                    {
                        new Point2D(0.0, 0.0),
                        new Point2D(100.0, 0.0),
                        new Point2D(100.0, 50.0),
                        new Point2D(0.0, 50.0)
                    };
                    CadHatch hatch1 = CreateRectangleHatch(1, 45.0, 5.0, rect1Vertices);

                    // Second hatched rectangle offset by (150,0)
                    var rect2Vertices = new List<Point2D>
                    {
                        new Point2D(150.0, 0.0),
                        new Point2D(250.0, 0.0),
                        new Point2D(250.0, 80.0),
                        new Point2D(150.0, 80.0)
                    };
                    CadHatch hatch2 = CreateRectangleHatch(2, 0.0, 3.0, rect2Vertices);

                    // Add hatches to the drawing
                    cadImage.Entities = cadImage.Entities
                        .Concat(new CadEntityBase[] { hatch1, hatch2 })
                        .ToArray();

                    // Prepare output path
                    string outputDirectory = Path.GetDirectoryName(inputPath);
                    if (string.IsNullOrEmpty(outputDirectory))
                    {
                        outputDirectory = Directory.GetCurrentDirectory();
                    }
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    string outputPath = Path.Combine(outputDirectory, "output_hatched.dxf");

                    // Save the modified drawing as DXF (ASCII format)
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Ascii
                    };
                    cadImage.Save(outputPath, dxfOptions);

                    Console.WriteLine($"Hatched drawing saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}