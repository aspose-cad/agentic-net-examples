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
        // Path to the source CAD file
        string sourcePath = "input.dwg";

        // Verify the source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD-specific collections
                CadImage cad = (CadImage)image;

                // List to store the file paths of IMAGEDEF definitions
                List<string> imageDefPaths = new List<string>();

                // Iterate through all CAD objects (raster image definitions are stored here)
                foreach (var obj in cad.Objects)
                {
                    if (obj is CadRasterImageDef rasterDef)
                    {
                        // Collect the current file name (full path) of the raster definition
                        imageDefPaths.Add(rasterDef.FileName);
                    }
                }

                // Output the collected IMAGEDEF file paths for auditing
                Console.WriteLine("Found IMAGEDEF file paths:");
                foreach (var path in imageDefPaths)
                {
                    Console.WriteLine(path);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}