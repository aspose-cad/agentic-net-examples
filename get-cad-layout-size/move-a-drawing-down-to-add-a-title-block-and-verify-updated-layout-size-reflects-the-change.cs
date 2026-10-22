using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadMoveDrawing
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file (replace with actual path)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Compute original layout bounding box
                    double originalMinX = double.MaxValue;
                    double originalMinY = double.MaxValue;
                    double originalMaxX = double.MinValue;
                    double originalMaxY = double.MinValue;

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        UpdateBounds(entity, ref originalMinX, ref originalMinY, ref originalMaxX, ref originalMaxY);
                    }

                    double originalWidth = originalMaxX - originalMinX;
                    double originalHeight = originalMaxY - originalMinY;

                    Console.WriteLine($"Original Layout Size: Width = {originalWidth}, Height = {originalHeight}");

                    // Define vertical offset to move drawing down (positive Y direction)
                    double offsetY = 100.0;

                    // Apply translation to each entity
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        TranslateEntityDown(entity, offsetY);
                    }

                    // Compute new layout bounding box after translation
                    double newMinX = double.MaxValue;
                    double newMinY = double.MaxValue;
                    double newMaxX = double.MinValue;
                    double newMaxY = double.MinValue;

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        UpdateBounds(entity, ref newMinX, ref newMinY, ref newMaxX, ref newMaxY);
                    }

                    double newWidth = newMaxX - newMinX;
                    double newHeight = newMaxY - newMinY;

                    Console.WriteLine($"Updated Layout Size: Width = {newWidth}, Height = {newHeight}");

                    double heightDifference = newHeight - originalHeight;
                    Console.WriteLine($"Height change after moving down: {heightDifference}");

                    // Save the modified drawing as PNG to visualize the result
                    string outputPath = "output.png";

                    // Determine first layout name if available
                    string firstLayoutName = cadImage.Layouts.Keys.FirstOrDefault() ?? string.Empty;

                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = string.IsNullOrEmpty(firstLayoutName) ? new string[0] : new[] { firstLayoutName },
                        PageWidth = 1600,
                        PageHeight = 1200,
                        DrawType = CadDrawTypeMode.UseObjectColor
                    };

                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    try
                    {
                        cadImage.Save(outputPath, pngOptions);
                        Console.WriteLine($"Modified drawing saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving output file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }

        // Updates the bounding box values based on the entity's geometry
        private static void UpdateBounds(CadEntityBase entity, ref double minX, ref double minY, ref double maxX, ref double maxY)
        {
            switch (entity)
            {
                case CadLine line:
                    UpdatePointBounds(line.FirstPoint, ref minX, ref minY, ref maxX, ref maxY);
                    UpdatePointBounds(line.SecondPoint, ref minX, ref minY, ref maxX, ref maxY);
                    break;

                case CadLwPolyline polyline:
                    foreach (Cad2DPoint point in polyline.Coordinates)
                    {
                        UpdatePointBounds(point, ref minX, ref minY, ref maxX, ref maxY);
                    }
                    break;

                case Cad3DFace face:
                    UpdatePointBounds(face.FirstCorner, ref minX, ref minY, ref maxX, ref maxY);
                    UpdatePointBounds(face.SecondCorner, ref minX, ref minY, ref maxX, ref maxY);
                    UpdatePointBounds(face.ThirdCorner, ref minX, ref minY, ref maxX, ref maxY);
                    UpdatePointBounds(face.FourthCorner, ref minX, ref minY, ref maxX, ref maxY);
                    break;

                case CadInsertObject insert:
                    if (insert.InsertionPoint != null)
                    {
                        UpdatePointBounds(insert.InsertionPoint, ref minX, ref minY, ref maxX, ref maxY);
                    }
                    break;
            }
        }

        // Translates the entity down by offsetY (adds to Y coordinate)
        private static void TranslateEntityDown(CadEntityBase entity, double offsetY)
        {
            switch (entity)
            {
                case CadLine line:
                    line.FirstPoint.Y += offsetY;
                    line.SecondPoint.Y += offsetY;
                    break;

                case CadLwPolyline polyline:
                    foreach (Cad2DPoint point in polyline.Coordinates)
                    {
                        point.Y += offsetY;
                    }
                    break;

                case Cad3DFace face:
                    face.FirstCorner.Y += offsetY;
                    face.SecondCorner.Y += offsetY;
                    face.ThirdCorner.Y += offsetY;
                    face.FourthCorner.Y += offsetY;
                    break;

                case CadInsertObject insert:
                    if (insert.InsertionPoint != null)
                    {
                        insert.InsertionPoint.Y += offsetY;
                    }
                    break;
            }
        }

        // Updates min/max values based on a 3D point
        private static void UpdatePointBounds(Cad3DPoint point, ref double minX, ref double minY, ref double maxX, ref double maxY)
        {
            if (point.X < minX) minX = point.X;
            if (point.Y < minY) minY = point.Y;
            if (point.X > maxX) maxX = point.X;
            if (point.Y > maxY) maxY = point.Y;
        }

        // Updates min/max values based on a 2D point
        private static void UpdatePointBounds(Cad2DPoint point, ref double minX, ref double minY, ref double maxX, ref double maxY)
        {
            if (point.X < minX) minX = point.X;
            if (point.Y < minY) minY = point.Y;
            if (point.X > maxX) maxX = point.X;
            if (point.Y > maxY) maxY = point.Y;
        }
    }
}