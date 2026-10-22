using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

class DeleteEntitiesInRectangle
{
    // Define rectangle bounds
    const double MinX = 0.0;
    const double MinY = 0.0;
    const double MaxX = 100.0;
    const double MaxY = 100.0;

    static void Main()
    {
        // Input and output paths (replace with actual paths or placeholders)
        var inputPath = "input.dxf";
        var outputPath = "output.dxf";

        // Validate input file existence
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load CAD image
            var cadImage = (CadImage)Image.Load(inputPath);

            // Process block entities
            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
            {
                var toRemove = new List<CadEntityBase>();

                foreach (var entity in block.Entities)
                {
                    if (IsInsideRectangle(entity))
                        toRemove.Add(entity);
                }

                if (toRemove.Count > 0)
                {
                    var remaining = block.Entities.Except(toRemove).ToArray();
                    block.Entities = remaining;
                }
            }

            // Process top‑level entities
            var topLevelToRemove = new List<CadEntityBase>();
            foreach (var entity in cadImage.Entities)
            {
                if (IsInsideRectangle(entity))
                    topLevelToRemove.Add(entity);
            }

            if (topLevelToRemove.Count > 0)
            {
                var filtered = cadImage.Entities.Except(topLevelToRemove).ToArray();
                cadImage.Entities = filtered;
            }

            // Save modified CAD image
            cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Determines whether a CAD entity is completely inside the defined rectangle
    static bool IsInsideRectangle(CadEntityBase entity)
    {
        bool PointInside2D(Cad2DPoint p) =>
            p.X >= MinX && p.X <= MaxX && p.Y >= MinY && p.Y <= MaxY;

        bool PointInside3D(Cad3DPoint p) =>
            p.X >= MinX && p.X <= MaxX && p.Y >= MinY && p.Y <= MaxY;

        switch (entity)
        {
            case CadLine line:
                return PointInside2D(line.FirstPoint) && PointInside2D(line.SecondPoint);

            case CadLwPolyline lwPolyline:
                return lwPolyline.Coordinates.All(PointInside2D);

            case CadPolyline polyline:
                var vertices = polyline.ChildObjects.OfType<CadVertexBase>();
                return vertices.All(v => PointInside3D(v.LocationPoint));

            // Add more entity types as needed
            default:
                return false;
        }
    }
}