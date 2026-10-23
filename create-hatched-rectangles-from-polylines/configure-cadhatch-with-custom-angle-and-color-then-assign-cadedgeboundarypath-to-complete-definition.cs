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

public class Program
{
    public static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        CadImage cadImage = null;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            return;
        }

        if (!cadImage.BlockEntities.TryGetValue("*MODEL_SPACE", out var modelSpaceObj) ||
            !(modelSpaceObj is CadBlockEntity modelSpace))
        {
            Console.WriteLine("Model space block not found.");
            cadImage?.Dispose();
            return;
        }

        var polylines = modelSpace.Entities
                                 .OfType<CadLwPolyline>()
                                 .ToList();

        foreach (var polyline in polylines)
        {
            var points = polyline.Coordinates
                                .Select(p => new Point2D(p.X, p.Y))
                                .ToList();

            if (points.Count < 2)
                continue;

            var edgeLines = new List<ICadBoundaryPathEntity>();
            var edgeTypes = new List<short>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                var line = new CadBoundaryPathLine
                {
                    FirstPoint = points[i],
                    SecondPoint = points[i + 1]
                };
                edgeLines.Add(line);
                edgeTypes.Add(1); // 1 = line
            }

            bool isClosed = (polyline.Flag & CadLwPolylineFlag.Closed) != 0;
            if (isClosed && points.Count > 2)
            {
                var line = new CadBoundaryPathLine
                {
                    FirstPoint = points[points.Count - 1],
                    SecondPoint = points[0]
                };
                edgeLines.Add(line);
                edgeTypes.Add(1);
            }

            var edgeBoundary = new CadEdgeBoundaryPath
            {
                NumberOfEdges = edgeLines.Count,
                EdgeTypes = edgeTypes,
                Objects = edgeLines
            };

            var boundaryContainer = new CadHatchBoundaryPathContainer
            {
                PathType = 0, // Edge boundary
                BoundaryPath = new List<ICadBoundaryPath> { edgeBoundary },
                SourceBoundaryObjects = new List<string>(),
                BoundaryObjectCount = 1
            };

            var hatch = new CadHatch
            {
                HatchAngle = 45.0,
                HatchScaleOrSpacing = 5.0,
                AssociativeFlag = 0,
                SolidFillFlag = 0,
                ColorId = 1,
                PatternName = "SOLID",
                OffsetVector = 0.0,
                Elevation = 0.0,
                SeedPoints = null,
                BoundaryPaths = new List<CadHatchBoundaryPathContainer> { boundaryContainer },
                PatternDefinitions = new List<CadHatchPatternData>()
            };

            modelSpace.AddEntity(hatch);
        }

        try
        {
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1100,
                    Layouts = new[] { "Model" }
                }
            };
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"Output saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save output file: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}