using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddMarginsToCadDrawings
{
    class Program
    {
        static void Main()
        {
            // Input CAD file path (replace with actual path)
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load CAD document
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Ensure layout extents are up‑to‑date
                    cadImage.UpdateSize(true);

                    // Iterate through layouts
                    foreach (KeyValuePair<string, CadLayout> kvp in cadImage.Layouts)
                    {
                        string layoutName = kvp.Key;
                        CadLayout layout = kvp.Value;

                        if (layout == null)
                            continue;

                        // Retrieve extents; handle possible nulls
                        var min = layout.MinExtents;
                        var max = layout.MaxExtents;

                        if (min == null || max == null)
                        {
                            Console.WriteLine($"Layout \"{layoutName}\" does not contain valid extents.");
                            continue;
                        }

                        double width = Math.Abs(max.X - min.X);
                        double height = Math.Abs(max.Y - min.Y);

                        // Calculate margins (example: 5% of dimensions)
                        double marginX = width * 0.05;
                        double marginY = height * 0.05;

                        Console.WriteLine($"Layout: {layoutName}");
                        Console.WriteLine($"  Width:  {width}");
                        Console.WriteLine($"  Height: {height}");
                        Console.WriteLine($"  Margin X: {marginX}");
                        Console.WriteLine($"  Margin Y: {marginY}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}