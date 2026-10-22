using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Input");
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            if (!Directory.Exists(inputFolder))
                Directory.CreateDirectory(inputFolder);
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string inputPath = Path.Combine(inputFolder, "sample.dwg");
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Define rectangle polyline (hatch region) – not added to drawing, only for geometry tests
                CadLwPolyline rectanglePolyline = new CadLwPolyline
                {
                    Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 50),
                        new Cad2DPoint(0, 50)
                    },
                    Flag = CadLwPolylineFlag.Closed,
                    LayerName = "RectLayer"
                };
                rectanglePolyline.PointCount = rectanglePolyline.Coordinates.Count;

                // Rectangle bounds for intersection tests
                double rectMinX = 0;
                double rectMaxX = 100;
                double rectMinY = 0;
                double rectMaxY = 50;

                List<CadEntityBase> intersectingEntities = new List<CadEntityBase>();

                // Helper local functions
                bool IsBoundingBoxIntersect(double minX1, double maxX1, double minY1, double maxY1,
                                            double minX2, double maxX2, double minY2, double maxY2)
                {
                    return !(maxX1 < minX2 || maxX2 < minX1 || maxY1 < minY2 || maxY2 < minY1);
                }

                void CheckEntity(CadEntityBase entity)
                {
                    if (entity is CadLine line)
                    {
                        double eMinX = Math.Min(line.FirstPoint.X, line.SecondPoint.X);
                        double eMaxX = Math.Max(line.FirstPoint.X, line.SecondPoint.X);
                        double eMinY = Math.Min(line.FirstPoint.Y, line.SecondPoint.Y);
                        double eMaxY = Math.Max(line.FirstPoint.Y, line.SecondPoint.Y);
                        if (IsBoundingBoxIntersect(eMinX, eMaxX, eMinY, eMaxY, rectMinX, rectMaxX, rectMinY, rectMaxY))
                            intersectingEntities.Add(entity);
                    }
                    else if (entity is CadCircle circle)
                    {
                        double eMinX = circle.CenterPoint.X - circle.Radius;
                        double eMaxX = circle.CenterPoint.X + circle.Radius;
                        double eMinY = circle.CenterPoint.Y - circle.Radius;
                        double eMaxY = circle.CenterPoint.Y + circle.Radius;
                        if (IsBoundingBoxIntersect(eMinX, eMaxX, eMinY, eMaxY, rectMinX, rectMaxX, rectMinY, rectMaxY))
                            intersectingEntities.Add(entity);
                    }
                    else if (entity is CadArc arc)
                    {
                        double eMinX = arc.CenterPoint.X - arc.Radius;
                        double eMaxX = arc.CenterPoint.X + arc.Radius;
                        double eMinY = arc.CenterPoint.Y - arc.Radius;
                        double eMaxY = arc.CenterPoint.Y + arc.Radius;
                        if (IsBoundingBoxIntersect(eMinX, eMaxX, eMinY, eMaxY, rectMinX, rectMaxX, rectMinY, rectMaxY))
                            intersectingEntities.Add(entity);
                    }
                    else if (entity is CadLwPolyline poly)
                    {
                        var pts = poly.Coordinates;
                        for (int i = 0; i < pts.Count - 1; i++)
                        {
                            double eMinX = Math.Min(pts[i].X, pts[i + 1].X);
                            double eMaxX = Math.Max(pts[i].X, pts[i + 1].X);
                            double eMinY = Math.Min(pts[i].Y, pts[i + 1].Y);
                            double eMaxY = Math.Max(pts[i].Y, pts[i + 1].Y);
                            if (IsBoundingBoxIntersect(eMinX, eMaxX, eMinY, eMaxY, rectMinX, rectMaxX, rectMinY, rectMaxY))
                            {
                                intersectingEntities.Add(entity);
                                break;
                            }
                        }
                    }
                    else if (entity is Cad3DFace face)
                    {
                        double eMinX = Math.Min(Math.Min(face.FirstCorner.X, face.SecondCorner.X), Math.Min(face.ThirdCorner.X, face.FourthCorner.X));
                        double eMaxX = Math.Max(Math.Max(face.FirstCorner.X, face.SecondCorner.X), Math.Max(face.ThirdCorner.X, face.FourthCorner.X));
                        double eMinY = Math.Min(Math.Min(face.FirstCorner.Y, face.SecondCorner.Y), Math.Min(face.ThirdCorner.Y, face.FourthCorner.Y));
                        double eMaxY = Math.Max(Math.Max(face.FirstCorner.Y, face.SecondCorner.Y), Math.Max(face.ThirdCorner.Y, face.FourthCorner.Y));
                        if (IsBoundingBoxIntersect(eMinX, eMaxX, eMinY, eMaxY, rectMinX, rectMaxX, rectMinY, rectMaxY))
                            intersectingEntities.Add(entity);
                    }
                }

                // Check top‑level entities
                foreach (CadEntityBase topEntity in cadImage.Entities)
                    CheckEntity(topEntity);

                // Check entities inside blocks
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase blockEntity in block.Entities)
                        CheckEntity(blockEntity);
                }

                // Output results
                Console.WriteLine($"Found {intersectingEntities.Count} intersecting entities:");
                foreach (CadEntityBase ent in intersectingEntities)
                {
                    string typeName = ent.GetType().Name;
                    Console.WriteLine($"- {typeName} on layer '{ent.LayerName}'");
                }

                // Save a copy of the original drawing to output folder
                string outputPath = Path.Combine(outputFolder, "sample_output.dwg");
                cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
                Console.WriteLine($"Drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}