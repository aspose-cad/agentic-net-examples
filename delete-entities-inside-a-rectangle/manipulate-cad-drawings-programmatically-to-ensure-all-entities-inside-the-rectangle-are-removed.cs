using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Define rectangle bounds (example values)
                double rectMinX = 0.0;
                double rectMinY = 0.0;
                double rectMaxX = 100.0;
                double rectMaxY = 100.0;

                // Collect entities that lie completely inside the rectangle
                List<CadEntityBase> entitiesToRemove = new List<CadEntityBase>();

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    bool inside = false;

                    if (entity is CadLine line)
                    {
                        inside = IsPointInside(line.FirstPoint, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                 IsPointInside(line.SecondPoint, rectMinX, rectMinY, rectMaxX, rectMaxY);
                    }
                    else if (entity is CadLwPolyline polyline)
                    {
                        inside = true;
                        foreach (Cad2DPoint pt in polyline.Coordinates)
                        {
                            if (!IsPointInside(pt, rectMinX, rectMinY, rectMaxX, rectMaxY))
                            {
                                inside = false;
                                break;
                            }
                        }
                    }
                    else if (entity is Cad3DFace face)
                    {
                        inside = IsPointInside(face.FirstCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                 IsPointInside(face.SecondCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                 IsPointInside(face.ThirdCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                 IsPointInside(face.FourthCorner, rectMinX, rectMinY, rectMaxX, rectMaxY);
                    }
                    // Additional entity types can be handled similarly

                    if (inside)
                    {
                        entitiesToRemove.Add(entity);
                    }
                }

                // Remove the identified entities from the drawing
                CadEntityBase[] remaining = cadImage.Entities.Except(entitiesToRemove).ToArray();
                cadImage.Entities = remaining;

                // Save the modified drawing as PNG
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { "Model" }
                };
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static bool IsPointInside(Cad3DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }

    static bool IsPointInside(Cad2DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }
}