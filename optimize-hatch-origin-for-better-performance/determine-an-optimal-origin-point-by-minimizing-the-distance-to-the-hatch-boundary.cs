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
using Aspose.CAD.FileFormats.Cad.CadConsts;

class OptimizeHatchOrigin
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            var cadImage = (CadImage)Image.Load(inputPath);

            // Process each hatch entity
            foreach (var hatch in cadImage.Entities.OfType<CadHatch>())
            {
                var allVertices = new List<Point2D>();

                if (hatch.BoundaryPaths != null)
                {
                    foreach (var container in hatch.BoundaryPaths)
                    {
                        if (container.BoundaryPath != null)
                        {
                            foreach (var path in container.BoundaryPath)
                            {
                                if (path is CadPolylineBoundaryPath polyPath && polyPath.Vertices != null)
                                {
                                    allVertices.AddRange(polyPath.Vertices);
                                }
                            }
                        }
                    }
                }

                if (allVertices.Count > 0)
                {
                    double centroidX = allVertices.Average(v => v.X);
                    double centroidY = allVertices.Average(v => v.Y);
                    var optimalOrigin = new Cad2DPoint(centroidX, centroidY);

                    if (hatch.PatternDefinitions != null)
                    {
                        foreach (var patternDef in hatch.PatternDefinitions)
                        {
                            if (patternDef != null)
                            {
                                patternDef.LineBasePoint = optimalOrigin;
                            }
                        }
                    }

                    // Example: change hatch color to magenta (true color)
                    hatch.ColorValue = 0xFF00FF;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the modified drawing
            var saveOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Ascii
            };
            cadImage.Save(outputPath, saveOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}