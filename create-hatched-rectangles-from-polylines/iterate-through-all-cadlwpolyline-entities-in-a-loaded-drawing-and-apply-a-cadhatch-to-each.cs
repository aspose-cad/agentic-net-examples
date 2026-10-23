using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                var lwPolylines = cadImage.Entities.OfType<CadLwPolyline>().ToList();

                foreach (CadLwPolyline polyline in lwPolylines)
                {
                    // Create a solid hatch
                    var hatch = new CadHatch
                    {
                        PatternName = "SOLID",
                        HatchAngle = 0.0,
                        HatchScaleOrSpacing = 1.0,
                        SolidFillFlag = (short)1,
                        ColorId = (short)1,
                        PatternDefinitions = new List<CadHatchPatternData>(),
                        SeedPoints = new List<Cad2DPoint>()
                    };

                    // Build polyline boundary path
                    var polyPath = new CadPolylineBoundaryPath
                    {
                        IsClosed = true,
                        Vertices = new List<Point2D>(),
                        Bugles = new List<double>(),
                        HasBugle = false
                    };

                    foreach (var pt in polyline.Coordinates)
                    {
                        polyPath.Vertices.Add(new Point2D(pt.X, pt.Y));
                    }
                    polyPath.MaxArrayLen = polyPath.Vertices.Count;

                    var container = new CadHatchBoundaryPathContainer
                    {
                        PathType = 2, // polyline boundary
                        BoundaryPath = new List<ICadBoundaryPath> { polyPath },
                        SourceBoundaryObjects = new List<string>()
                    };

                    hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { container };

                    // Append hatch to drawing entities
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { hatch }).ToArray();
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };

                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}