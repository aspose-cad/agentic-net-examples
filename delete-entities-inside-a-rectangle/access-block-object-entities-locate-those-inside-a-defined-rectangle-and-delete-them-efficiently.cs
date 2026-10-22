using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Input and output CAD file paths
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dwg";

                // Rectangle bounds (minX, minY, maxX, maxY)
                double rectMinX = 0.0;
                double rectMinY = 0.0;
                double rectMaxX = 100.0;
                double rectMaxY = 100.0;

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Iterate over each block definition in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // List to hold entities that need to be removed
                        List<CadEntityBase> entitiesToRemove = new List<CadEntityBase>();

                        // Examine each entity inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            bool inside = false;

                            // Determine if the entity lies completely within the rectangle
                            switch (entity)
                            {
                                case CadLine line:
                                    // Both end points must be inside the rectangle
                                    inside = IsPointInRect(line.FirstPoint, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                             IsPointInRect(line.SecondPoint, rectMinX, rectMinY, rectMaxX, rectMaxY);
                                    break;

                                case CadLwPolyline polyline:
                                    // If any vertex is inside, consider the polyline for removal
                                    foreach (Cad2DPoint pt in polyline.Coordinates)
                                    {
                                        if (IsPointInRect(pt, rectMinX, rectMinY, rectMaxX, rectMaxY))
                                        {
                                            inside = true;
                                            break;
                                        }
                                    }
                                    break;

                                case Cad3DFace face:
                                    // All four corners must be inside the rectangle
                                    inside = IsPointInRect(face.FirstCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                             IsPointInRect(face.SecondCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                             IsPointInRect(face.ThirdCorner, rectMinX, rectMinY, rectMaxX, rectMaxY) &&
                                             IsPointInRect(face.FourthCorner, rectMinX, rectMinY, rectMaxX, rectMaxY);
                                    break;

                                // Other entity types can be added here as needed
                                default:
                                    break;
                            }

                            if (inside)
                            {
                                entitiesToRemove.Add(entity);
                            }
                        }

                        // Remove the collected entities from the block
                        foreach (CadEntityBase ent in entitiesToRemove)
                        {
                            block.Entities.Remove(ent);
                        }
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

    // Helper for 3D points
    static bool IsPointInRect(Cad3DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }

    // Helper for 2D points
    static bool IsPointInRect(Cad2DPoint point, double minX, double minY, double maxX, double maxY)
    {
        return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
    }
}