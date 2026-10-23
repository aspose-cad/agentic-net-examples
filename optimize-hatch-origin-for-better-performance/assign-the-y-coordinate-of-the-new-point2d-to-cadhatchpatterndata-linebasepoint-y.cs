using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Create a new CadHatchPatternData instance
                CadHatchPatternData hatchData = new CadHatchPatternData();

                // Create a new Cad2DPoint with desired coordinates
                Cad2DPoint basePoint = new Cad2DPoint(10.0, 20.0);

                // Assign the Y coordinate of the new point to LineBasePoint.Y
                hatchData.LineBasePoint = basePoint;
                hatchData.LineBasePoint.Y = basePoint.Y; // explicit Y assignment

                // Build a simple hatch entity to demonstrate usage
                CadHatch hatch = new CadHatch
                {
                    PatternName = "SOLID",
                    SolidFillFlag = 1,
                    ColorId = 7 // white indexed color
                };

                // Prepare boundary path container (rectangle)
                CadHatchBoundaryPathContainer container = new CadHatchBoundaryPathContainer
                {
                    PathType = 2, // polyline
                    BoundaryPath = new List<ICadBoundaryPath>(),
                    SourceBoundaryObjects = new List<string>(),
                    BoundaryObjectCount = 0
                };

                CadPolylineBoundaryPath polyPath = new CadPolylineBoundaryPath
                {
                    IsClosed = true,
                    Vertices = new List<Point2D>
                    {
                        new Point2D(0, 0),
                        new Point2D(100, 0),
                        new Point2D(100, 100),
                        new Point2D(0, 100)
                    },
                    MaxArrayLen = 4,
                    Bugles = new List<double>(),
                    HasBugle = false
                };

                container.BoundaryPath.Add(polyPath);
                hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { container };
                hatch.PatternDefinitions = new List<CadHatchPatternData> { hatchData };

                // Add hatch to model space if available
                if (cadImage.BlockEntities.TryGetValue("*Model_Space", out var modelBlock))
                {
                    modelBlock.AddEntity(hatch);
                }
                else
                {
                    Console.Error.WriteLine("Model space block not found; cannot add hatch.");
                }

                // Save the modified CAD image
                DwgOptions saveOptions = new DwgOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        UnitType = UnitType.Millimeter
                    }
                };

                cadImage.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}