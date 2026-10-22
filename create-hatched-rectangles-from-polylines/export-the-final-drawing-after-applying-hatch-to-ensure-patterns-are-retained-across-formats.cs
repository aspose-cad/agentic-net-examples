using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Iterate through all block entities
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Find all hatch entities within the block
                    var hatches = block.Entities.OfType<CadHatch>().ToList();

                    foreach (CadHatch hatch in hatches)
                    {
                        // Change hatch color (true color magenta)
                        hatch.ColorValue = 0xFF00FF;

                        // Set hatch pattern and scale
                        hatch.PatternName = "ANSI31";
                        hatch.HatchScaleOrSpacing = 0.5;
                        hatch.HatchAngle = 0.0;

                        // Ensure boundary paths collection exists
                        if (hatch.BoundaryPaths == null)
                            hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer>();

                        // If no boundary, create a rectangular polyline boundary
                        if (!hatch.BoundaryPaths.Any())
                        {
                            // Create polyline boundary path
                            var polyPath = new CadPolylineBoundaryPath
                            {
                                IsClosed = true,
                                Vertices = new List<Point2D>
                                {
                                    new Point2D(0, 0),
                                    new Point2D(100, 0),
                                    new Point2D(100, 100),
                                    new Point2D(0, 100)
                                }
                            };
                            polyPath.MaxArrayLen = polyPath.Vertices.Count;

                            // Create container for the boundary path
                            var container = new CadHatchBoundaryPathContainer
                            {
                                PathType = 2, // Polyline
                                BoundaryPath = new List<ICadBoundaryPath> { polyPath },
                                SourceBoundaryObjects = new List<string>()
                            };

                            hatch.BoundaryPaths.Add(container);
                        }
                    }
                }

                // Save the modified drawing preserving hatch patterns
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine("Export completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}