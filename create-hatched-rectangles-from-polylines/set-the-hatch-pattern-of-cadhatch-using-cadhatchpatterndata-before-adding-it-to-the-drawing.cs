using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.dxf";
            string outputPath = "output.dxf";

            // Guard against missing input file
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
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a new hatch entity
                CadHatch hatch = new CadHatch();

                // Set basic hatch properties
                hatch.ColorId = 1;                     // Indexed color (e.g., red)
                hatch.HatchAngle = 45.0;               // Hatch rotation angle
                hatch.HatchScaleOrSpacing = 0.5;       // Hatch scale / spacing

                // Define the hatch pattern data (single line pattern)
                CadHatchPatternData patternData = new CadHatchPatternData
                {
                    LineAngle = 0.0,
                    LineBasePoint = new Cad2DPoint(0, 0),
                    LineOffset = new Cad2DPoint(0, 0),
                    DashLengthCount = 0,
                    DashLengths = new List<double>()
                };

                // Assign the pattern definition to the hatch
                hatch.PatternDefinitions = new List<CadHatchPatternData> { patternData };

                // Create a boundary path container for the hatch
                CadHatchBoundaryPathContainer boundaryContainer = new CadHatchBoundaryPathContainer
                {
                    PathType = 2, // Polyline type
                    BoundaryPath = new List<ICadBoundaryPath>(),
                    SourceBoundaryObjects = new List<string>()
                };

                // Define a rectangular polyline boundary
                CadPolylineBoundaryPath polyBoundary = new CadPolylineBoundaryPath
                {
                    Vertices = new List<Point2D>
                    {
                        new Point2D(0, 0),
                        new Point2D(100, 0),
                        new Point2D(100, 100),
                        new Point2D(0, 100)
                    },
                    IsClosed = true
                };
                // Record vertex count
                polyBoundary.MaxArrayLen = polyBoundary.Vertices.Count;

                // Add the polyline boundary to the container
                boundaryContainer.BoundaryPath.Add(polyBoundary);

                // Assign the boundary container to the hatch
                hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { boundaryContainer };

                // Add the hatch to the drawing's entity collection
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { hatch }).ToArray();

                // Save the modified drawing
                cadImage.Save(outputPath);
            }

            Console.WriteLine($"Hatched drawing saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}