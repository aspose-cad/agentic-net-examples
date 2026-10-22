using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace DeleteEntitiesInRectangle
{
    class Program
    {
        // Rectangle bounds (world coordinates)
        private const double MinX = 0.0;
        private const double MinY = 0.0;
        private const double MaxX = 100.0;
        private const double MaxY = 100.0;

        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths
                string inputPath = @"input.dwg";
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                string outputPath = @"output.dwg";

                // Ensure output directory exists
                string outDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
                    Directory.CreateDirectory(outDir);

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Process top‑level entities
                    var topLevel = cadImage.Entities.ToList();
                    RemoveEntitiesInsideRectangle(topLevel);
                    // Replace the collection with the filtered list
                    cadImage.Entities = topLevel;

                    // Process entities inside each block definition
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        var blockEntities = block.Entities.ToList();
                        RemoveEntitiesInsideRectangle(blockEntities);
                        // Clear existing and add back filtered entities
                        block.Entities.Clear();
                        foreach (var ent in blockEntities)
                            block.AddEntity(ent);
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                    Console.WriteLine($"Saved modified drawing to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }

        // Removes entities from the supplied list that lie inside the rectangle
        private static void RemoveEntitiesInsideRectangle(IList<CadEntityBase> entities)
        {
            var toRemove = new List<CadEntityBase>();

            foreach (var entity in entities)
            {
                if (IsEntityInsideRectangle(entity))
                    toRemove.Add(entity);
            }

            foreach (var entity in toRemove)
                entities.Remove(entity);
        }

        // Determines whether the given entity is inside the rectangle
        private static bool IsEntityInsideRectangle(CadEntityBase entity)
        {
            switch (entity)
            {
                case CadArc arc:
                    // Simple check: center inside rectangle
                    return PointInside(arc.CenterPoint);

                case CadCircle circle:
                    // Simple check: center inside rectangle
                    return PointInside(circle.CenterPoint);

                case CadLine line:
                    return PointInside(line.FirstPoint) && PointInside(line.SecondPoint);

                case CadLwPolyline polyline:
                    foreach (var pt in polyline.Coordinates)
                    {
                        if (!PointInside(pt))
                            return false;
                    }
                    return true;

                case Cad3DFace face:
                    return PointInside(face.FirstCorner) &&
                           PointInside(face.SecondCorner) &&
                           PointInside(face.ThirdCorner) &&
                           PointInside(face.FourthCorner);

                default:
                    // Unknown entity types are left untouched
                    return false;
            }
        }

        // Checks whether a 2‑D point lies inside the rectangle
        private static bool PointInside(Cad2DPoint pt)
        {
            return pt.X >= MinX && pt.X <= MaxX && pt.Y >= MinY && pt.Y <= MaxY;
        }

        // Overload for 3‑D points (Z is ignored for rectangle test)
        private static bool PointInside(Cad3DPoint pt)
        {
            return pt.X >= MinX && pt.X <= MaxX && pt.Y >= MinY && pt.Y <= MaxY;
        }
    }
}