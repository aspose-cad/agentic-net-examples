using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddBackgroundImageAndText
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DXF file
                string sourcePath = "input.dxf";

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the DXF file as a CadImage
                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Calculate the bounding box that encloses all entities in the drawing
                    double minX = double.MaxValue;
                    double minY = double.MaxValue;
                    double maxX = double.MinValue;
                    double maxY = double.MinValue;

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Use reflection to avoid direct reference to System.Drawing.RectangleF
                        var bounds = entity.Bounds;
                        var boundsType = bounds.GetType();

                        double left = Convert.ToDouble(boundsType.GetProperty("Left")?.GetValue(bounds) ?? 0);
                        double top = Convert.ToDouble(boundsType.GetProperty("Top")?.GetValue(bounds) ?? 0);
                        double right = Convert.ToDouble(boundsType.GetProperty("Right")?.GetValue(bounds) ?? 0);
                        double bottom = Convert.ToDouble(boundsType.GetProperty("Bottom")?.GetValue(bounds) ?? 0);

                        if (left < minX) minX = left;
                        if (top < minY) minY = top;
                        if (right > maxX) maxX = right;
                        if (bottom > maxY) maxY = bottom;
                    }

                    // If no entities were found, the layout size is zero
                    if (minX == double.MaxValue || minY == double.MaxValue ||
                        maxX == double.MinValue || maxY == double.MinValue)
                    {
                        Console.WriteLine("The drawing contains no entities.");
                    }
                    else
                    {
                        double width = maxX - minX;
                        double height = maxY - minY;

                        Console.WriteLine("Layout size after processing:");
                        Console.WriteLine($"Width : {width} units");
                        Console.WriteLine($"Height: {height} units");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}