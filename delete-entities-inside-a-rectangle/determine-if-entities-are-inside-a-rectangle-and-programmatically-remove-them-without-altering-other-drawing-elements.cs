using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

class DeleteEntitiesInsideRectangle
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            var cadImage = (CadImage)Image.Load(inputPath);

            // Define rectangle bounds (inclusive)
            double rectMinX = 0.0;
            double rectMinY = 0.0;
            double rectMaxX = 100.0;
            double rectMaxY = 100.0;

            // Helper to test if a 2D point lies inside the rectangle
            bool IsPointInside(double x, double y) =>
                x >= rectMinX && x <= rectMaxX && y >= rectMinY && y <= rectMaxY;

            // Process top‑level entities
            var retainedEntities = new List<CadEntityBase>();

            foreach (var entity in cadImage.Entities)
            {
                bool inside = false;

                switch (entity)
                {
                    case CadLine line:
                        inside = IsPointInside(line.FirstPoint.X, line.FirstPoint.Y) &&
                                 IsPointInside(line.SecondPoint.X, line.SecondPoint.Y);
                        break;

                    case CadCircle circle:
                        inside = IsPointInside(circle.CenterPoint.X, circle.CenterPoint.Y);
                        break;

                    case CadLwPolyline lwPoly:
                        inside = lwPoly.Coordinates.All(pt => IsPointInside(pt.X, pt.Y));
                        break;

                    case CadPolyline poly3D:
                        var vertices3D = poly3D.ChildObjects
                            .OfType<CadVertexBase>()
                            .Select(v => v.LocationPoint);
                        inside = vertices3D.All(p => IsPointInside(p.X, p.Y));
                        break;

                    default:
                        inside = false;
                        break;
                }

                if (!inside)
                    retainedEntities.Add(entity);
            }

            cadImage.Entities = retainedEntities.ToArray();

            // Process entities inside each block (if any)
            foreach (var block in cadImage.BlockEntities.ValuesTyped)
            {
                var retainedBlockEntities = new List<CadEntityBase>();

                foreach (var blockEntity in block.Entities)
                {
                    bool inside = false;

                    switch (blockEntity)
                    {
                        case CadLine line:
                            inside = IsPointInside(line.FirstPoint.X, line.FirstPoint.Y) &&
                                     IsPointInside(line.SecondPoint.X, line.SecondPoint.Y);
                            break;

                        case CadCircle circle:
                            inside = IsPointInside(circle.CenterPoint.X, circle.CenterPoint.Y);
                            break;

                        case CadLwPolyline lwPoly:
                            inside = lwPoly.Coordinates.All(pt => IsPointInside(pt.X, pt.Y));
                            break;

                        case CadPolyline poly3D:
                            var vertices = poly3D.ChildObjects
                                .OfType<CadVertexBase>()
                                .Select(v => v.LocationPoint);
                            inside = vertices.All(p => IsPointInside(p.X, p.Y));
                            break;

                        default:
                            inside = false;
                            break;
                    }

                    if (!inside)
                        retainedBlockEntities.Add(blockEntity);
                }

                block.Entities = retainedBlockEntities.ToArray();
            }

            // Save the modified CAD file
            cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}