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
            // Path to the source CAD file
            string sourcePath = "input.dwg";

            // Guard against missing file
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(sourcePath))
            {
                if (image is CadImage cadImage)
                {
                    // Retrieve the collection of layouts
                    IList<CadLayout> layouts = new List<CadLayout>(cadImage.Layouts.Values);

                    // Output layout names
                    foreach (CadLayout layout in layouts)
                    {
                        Console.WriteLine($"Layout name: {layout.LayoutName}");
                    }
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}