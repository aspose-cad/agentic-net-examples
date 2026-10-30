using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;
using Aspose.CAD.ImageOptions;

class ReplaceCadHatch
{
    static void Main(string[] args)
    {
        // Input and output file paths (replace with actual paths)
        string inputPath = @"C:\Input\sample.dxf";
        string outputPath = @"C:\Output\sample_modified.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Model space block name is typically "*Model_Space"
            const string modelSpaceName = "*Model_Space";
            if (!cadImage.BlockEntities.ContainsKey(modelSpaceName))
            {
                Console.WriteLine("Model space block not found.");
                return;
            }

            CadBlockEntity modelSpace = cadImage.BlockEntities[modelSpaceName];

            // Find the first hatch entity
            var oldHatch = modelSpace.Entities.OfType<CadHatch>().FirstOrDefault();

            if (oldHatch == null)
            {
                Console.WriteLine("No hatch entity found to replace.");
                return;
            }

            // Remove the old hatch
            bool removed = modelSpace.Entities.Remove(oldHatch);
            if (!removed)
            {
                Console.WriteLine("Failed to remove the existing hatch entity.");
                return;
            }

            // Create a new hatch with a different pattern name and parameters
            var newHatch = new CadHatch
            {
                PatternName = "ANSI31",
                HatchAngle = 45.0,
                HatchScaleOrSpacing = 0.5,
                AssociativeFlag = 1
            };

            // Define custom pattern data (example: simple line pattern)
            var patternData = new CadHatchPatternData
            {
                LineAngle = 45.0,
                DashLengthCount = 0,
                DashLengths = new List<double>()
            };
            newHatch.PatternDefinitions = new List<CadHatchPatternData> { patternData };

            // Define a rectangular polyline boundary for the hatch
            var polyBoundary = new CadPolylineBoundaryPath
            {
                IsClosed = true,
                MaxArrayLen = 0,
                Bugles = new List<double>(),
                HasBugle = false
            };
            polyBoundary.Vertices.Add(new Point2D(0, 0));
            polyBoundary.Vertices.Add(new Point2D(100, 0));
            polyBoundary.Vertices.Add(new Point2D(100, 50));
            polyBoundary.Vertices.Add(new Point2D(0, 50));
            polyBoundary.MaxArrayLen = polyBoundary.Vertices.Count;

            var boundaryContainer = new CadHatchBoundaryPathContainer
            {
                PathType = 2, // Polyline boundary
                BoundaryPath = new List<ICadBoundaryPath> { polyBoundary },
                SourceBoundaryObjects = new List<string>()
            };

            newHatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { boundaryContainer };

            // Add the new hatch to the model space
            modelSpace.AddEntity(newHatch);

            // Save the modified drawing
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Ascii
            };
            cadImage.Save(outputPath, dxfOptions);

            Console.WriteLine($"Hatch replaced and file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}