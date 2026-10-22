using System;
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
            // Input CAD file path
            string inputPath = "sample.dwg";
            // Name of the layout to inspect
            string layoutName = "Layout1";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD document
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Locate the requested layout
                CadLayout targetLayout = null;
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    if (string.Equals(layout.LayoutName, layoutName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetLayout = layout;
                        break;
                    }
                }

                if (targetLayout == null)
                {
                    Console.Error.WriteLine($"Layout '{layoutName}' not found.");
                    return;
                }

                // Calculate layout dimensions using extents
                double width = targetLayout.MaxExtents.X - targetLayout.MinExtents.X;
                double height = targetLayout.MaxExtents.Y - targetLayout.MinExtents.Y;

                // Output layout dimensions
                Console.WriteLine($"Layout '{targetLayout.LayoutName}' size:");
                Console.WriteLine($"Width: {width} units");
                Console.WriteLine($"Height: {height} units");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}