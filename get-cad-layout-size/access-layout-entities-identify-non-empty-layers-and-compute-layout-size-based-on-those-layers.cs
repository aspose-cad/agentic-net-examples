using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                // Iterate through each layout in the drawing
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    // CadLayout does not expose a Name property; use LayoutName if available, otherwise fallback to type name
                    string layoutName = layout.GetType().GetProperty("LayoutName")?.GetValue(layout) as string
                                        ?? layout.GetType().Name;

                    Console.WriteLine($"Layout: {layoutName}");

                    // Set to store names of layers that contain at least one entity
                    HashSet<string> nonEmptyLayers = new HashSet<string>();

                    // Variables for bounding box calculation
                    double minX = double.MaxValue;
                    double minY = double.MaxValue;
                    double maxX = double.MinValue;
                    double maxY = double.MinValue;

                    // Local function to update bounding box limits
                    void UpdateBounds(double x, double y)
                    {
                        if (x < minX) minX = x;
                        if (y < minY) minY = y;
                        if (x > maxX) maxX = x;
                        if (y > maxY) maxY = y;
                    }

                    // Examine each entity in the current layout
                    foreach (CadEntityBase entity in layout.Entities)
                    {
                        // Record the layer name
                        string layerName = entity.LayerName;
                        nonEmptyLayers.Add(layerName);

                        // Update bounding box based on entity geometry
                        switch (entity)
                        {
                            case CadLine line:
                                UpdateBounds(line.FirstPoint.X, line.FirstPoint.Y);
                                UpdateBounds(line.SecondPoint.X, line.SecondPoint.Y);
                                break;

                            case CadLwPolyline polyline:
                                foreach (Cad2DPoint vertex in polyline.Coordinates)
                                {
                                    UpdateBounds(vertex.X, vertex.Y);
                                }
                                break;

                            case CadCircle circle:
                                double cx = circle.CenterPoint.X;
                                double cy = circle.CenterPoint.Y;
                                double r = circle.Radius;
                                UpdateBounds(cx - r, cy - r);
                                UpdateBounds(cx + r, cy + r);
                                break;

                            // Additional entity types can be handled here as needed
                        }
                    }

                    // Output the list of non‑empty layers
                    Console.WriteLine("Non‑empty layers:");
                    foreach (string layer in nonEmptyLayers)
                    {
                        Console.WriteLine($"  {layer}");
                    }

                    // Compute layout size based on the collected bounding box
                    double width = (maxX == double.MinValue) ? 0 : maxX - minX;
                    double height = (maxY == double.MinValue) ? 0 : maxY - minY;
                    Console.WriteLine($"Layout size based on non‑empty layers: Width = {width}, Height = {height}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}