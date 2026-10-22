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

class ReplaceRectangleFillWithHatch
{
    static void Main()
    {
        // Input and output paths (replace with actual paths)
        string inputPath = @"C:\CAD\input.dxf";
        string outputPath = @"C:\CAD\output.dxf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Find lightweight polylines that represent rectangles (4 vertices)
                var rectangles = cadImage.Entities
                    .OfType<CadLwPolyline>()
                    .Where(pl => pl.Coordinates != null && pl.Coordinates.Count == 4)
                    .ToList();

                foreach (var rect in rectangles)
                {
                    // Create a new hatch
                    CadHatch hatch = new CadHatch
                    {
                        SoftOwner = rect.SoftOwner,
                        SolidFillFlag = (short)1,          // solid fill
                        ColorId = (short)7,                // indexed color (white)
                        HatchAngle = 0.0,
                        HatchScaleOrSpacing = 1.0
                    };

                    // Prepare boundary path container
                    var container = new CadHatchBoundaryPathContainer
                    {
                        PathType = 2, // polyline boundary
                        BoundaryPath = new List<ICadBoundaryPath>(),
                        SourceBoundaryObjects = new List<string>()
                    };

                    // Create polyline boundary matching rectangle vertices
                    var polyBoundary = new CadPolylineBoundaryPath
                    {
                        Vertices = rect.Coordinates
                            .Select(c => new Point2D(c.X, c.Y))
                            .ToList(),
                        MaxArrayLen = rect.Coordinates.Count,
                        IsClosed = true,
                        Bugles = new List<double>(),
                        HasBugle = false
                    };

                    // Add the polyline boundary to the container
                    container.BoundaryPath.Add(polyBoundary);

                    // Assign the boundary container to the hatch
                    hatch.BoundaryPaths = new List<CadHatchBoundaryPathContainer> { container };

                    // Try to add the hatch to the same block/layout as the rectangle
                    bool addedToBlock = false;
                    if (cadImage.BlockEntities != null &&
                        cadImage.BlockEntities.TryGetValue(rect.SoftOwner, out var block))
                    {
                        block.AddEntity(hatch);
                        addedToBlock = true;
                    }

                    // Fallback: add to model space entities via replacement
                    if (!addedToBlock)
                    {
                        var updated = cadImage.Entities
                            .Cast<CadEntityBase>()
                            .Concat(new[] { hatch })
                            .ToArray();

                        // The Entities property can be reassigned in this context
                        cadImage.Entities = updated;
                    }
                }

                // Save the modified drawing as binary DXF
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}