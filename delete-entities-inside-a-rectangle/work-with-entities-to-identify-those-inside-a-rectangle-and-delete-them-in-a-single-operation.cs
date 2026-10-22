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
        const string outputPath = "output.pdf";

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
                List<CadEntityBase> entitiesToDelete = new List<CadEntityBase>();

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (IsEntityInsideRectangle(entity, rectMinX, rectMinY, rectMaxX, rectMaxY))
                    {
                        entitiesToDelete.Add(entity);
                    }
                }

                // Delete the identified entities in a single operation
                CadEntityBase[] remaining = cadImage.Entities
                    .Where(e => !entitiesToDelete.Contains(e))
                    .ToArray();

                // Replace the Entities collection with the filtered one
                cadImage.Entities = remaining;

                // Save the modified drawing as PDF
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { "Model" }
                };
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Determines whether a CAD entity is completely inside the specified rectangle
    static bool IsEntityInsideRectangle(CadEntityBase entity, double minX, double minY, double maxX, double maxY)
    {
        // Line entity
        if (entity is CadLine line)
        {
            return PointInside(line.FirstPoint, minX, minY, maxX, maxY) &&
                   PointInside(line.SecondPoint, minX, minY, maxX, maxY);
        }

        // Lightweight polyline entity
        if (entity is CadLwPolyline polyline)
        {
            foreach (Cad2DPoint pt in polyline.Coordinates)
            {
                if (!PointInside2D(pt, minX, minY, maxX, maxY))
                    return false;
            }
            return true;
        }

        // 3D Face entity
        if (entity is Cad3DFace face)
        {
            return PointInside(face.FirstCorner, minX, minY, maxX, maxY) &&
                   PointInside(face.SecondCorner, minX, minY, maxX, maxY) &&
                   PointInside(face.ThirdCorner, minX, minY, maxX, maxY) &&
                   PointInside(face.FourthCorner, minX, minY, maxX, maxY);
        }

        // Insert object entity (check insertion point)
        if (entity is CadInsertObject insert)
        {
            return PointInside(insert.InsertionPoint, minX, minY, maxX, maxY);
        }

        // Other entity types are considered outside the rectangle
        return false;
    }

    // Checks if a 3D point lies within the rectangle (ignores Z)
    static bool PointInside(Cad3DPoint pt, double minX, double minY, double maxX, double maxY)
    {
        return pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y <= maxY;
    }

    // Checks if a 2D point lies within the rectangle
    static bool PointInside2D(Cad2DPoint pt, double minX, double minY, double maxX, double maxY)
    {
        return pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y <= maxY;
    }
}