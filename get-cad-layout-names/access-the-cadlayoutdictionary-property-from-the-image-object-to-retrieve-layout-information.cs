using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        // Path to the source CAD file
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Load the CAD image
        using (Image image = Image.Load(inputPath))
        {
            // Cast to CadImage to access CAD‑specific properties
            CadImage cadImage = image as CadImage;
            if (cadImage == null)
            {
                Console.WriteLine("Failed to load CAD image.");
                return;
            }

            // Retrieve the layouts collection (read‑only dictionary of layout name -> CadLayout)
            var layouts = cadImage.Layouts;

            if (layouts == null || layouts.Count == 0)
            {
                Console.WriteLine("No layouts found in the CAD file.");
                return;
            }

            // Iterate through the layouts and output their names
            foreach (KeyValuePair<string, CadLayout> layoutEntry in layouts)
            {
                Console.WriteLine($"Layout name: {layoutEntry.Key}");
            }
        }
    }
}