using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file
            string sourcePath = "input.dwg";

            // Name of the layout whose layers we want to list
            string layoutName = "Layout1";

            // Guard against missing input file
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(sourcePath))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The file is not a CAD image.");
                    return;
                }

                // Find the requested layout by name
                CadLayout targetLayout = null;
                foreach (var kvp in cadImage.Layouts)
                {
                    CadLayout layout = kvp.Value;
                    if (string.Equals(layout.LayoutName, layoutName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetLayout = layout;
                        break;
                    }
                }

                if (targetLayout == null)
                {
                    Console.WriteLine($"Layout '{layoutName}' not found.");
                    return;
                }

                // List distinct layer names used by entities in the found layout
                var layerNames = targetLayout.Entities
                                            .Select(e => e.LayerName)
                                            .Where(name => !string.IsNullOrEmpty(name))
                                            .Distinct()
                                            .OrderBy(name => name)
                                            .ToList();

                Console.WriteLine($"Layers in layout '{layoutName}':");
                if (layerNames.Count == 0)
                {
                    Console.WriteLine("(No layers found in this layout.)");
                }
                else
                {
                    foreach (var name in layerNames)
                    {
                        Console.WriteLine(name);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}