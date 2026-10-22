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
        try
        {
            // Path to the DWG file
            string sourcePath = "sample.dwg";

            // Verify that the file exists
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"File not found: {sourcePath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Determine which layers contain at least one entity
                HashSet<string> nonEmptyLayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        nonEmptyLayers.Add(entity.LayerName);
                    }
                }

                // List non‑empty layers
                Console.WriteLine("Non‑empty layers:");
                foreach (string layerName in nonEmptyLayers)
                {
                    Console.WriteLine($"- {layerName}");
                }

                // If there are non‑empty layers, output layout dimensions
                if (nonEmptyLayers.Count > 0)
                {
                    foreach (CadLayout layout in cadImage.Layouts.Values)
                    {
                        // Calculate width and height from extents
                        double width = layout.MaxExtents.X - layout.MinExtents.X;
                        double height = layout.MaxExtents.Y - layout.MinExtents.Y;

                        Console.WriteLine($"Layout \"{layout.LayoutName}\": Width = {width}, Height = {height}");
                    }
                }
                else
                {
                    Console.WriteLine("No non‑empty layers found; layout size calculation skipped.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}