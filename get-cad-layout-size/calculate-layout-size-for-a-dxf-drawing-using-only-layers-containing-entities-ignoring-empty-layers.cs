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
        string sourcePath = "input.dxf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"File not found: {sourcePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(sourcePath))
            {
                var cadImage = (CadImage)image;

                bool hasAnyEntity = false;
                double minX = double.MaxValue, minY = double.MaxValue;
                double maxX = double.MinValue, maxY = double.MinValue;

                // Determine layers that actually contain entities
                var nonEmptyLayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        nonEmptyLayers.Add(entity.LayerName);
                    }
                }

                // Process entities that belong to non‑empty layers
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (string.IsNullOrEmpty(entity.LayerName) || !nonEmptyLayers.Contains(entity.LayerName))
                        continue;

                    object bounds = entity.Bounds;
                    if (bounds == null)
                        continue;

                    double left = Convert.ToDouble(bounds.GetType().GetProperty("Left")?.GetValue(bounds) ?? 0);
                    double top = Convert.ToDouble(bounds.GetType().GetProperty("Top")?.GetValue(bounds) ?? 0);
                    double right = Convert.ToDouble(bounds.GetType().GetProperty("Right")?.GetValue(bounds) ?? 0);
                    double bottom = Convert.ToDouble(bounds.GetType().GetProperty("Bottom")?.GetValue(bounds) ?? 0);

                    if (left < minX) minX = left;
                    if (top < minY) minY = top;
                    if (right > maxX) maxX = right;
                    if (bottom > maxY) maxY = bottom;

                    hasAnyEntity = true;
                }

                if (hasAnyEntity)
                {
                    double width = maxX - minX;
                    double height = maxY - minY;
                    Console.WriteLine($"Layout size (non‑empty layers): Width = {width}, Height = {height}");
                }
                else
                {
                    Console.WriteLine("The drawing contains no entities in any layer.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}