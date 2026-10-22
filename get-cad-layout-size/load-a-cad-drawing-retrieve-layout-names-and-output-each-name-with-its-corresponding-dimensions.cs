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
            // Define input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Check if the drawing contains any layouts
                if (cadImage.Layouts == null || cadImage.Layouts.Count == 0)
                {
                    Console.WriteLine("No layouts found in the drawing.");
                }
                else
                {
                    // Iterate through each layout and output its name and dimensions
                    foreach (KeyValuePair<string, CadLayout> layoutEntry in cadImage.Layouts)
                    {
                        CadLayout layout = layoutEntry.Value;

                        // Retrieve layout dimensions from extents
                        double width = layout.MaxExtents.X - layout.MinExtents.X;
                        double height = layout.MaxExtents.Y - layout.MinExtents.Y;

                        Console.WriteLine($"Layout: {layout.LayoutName}, Width: {width}, Height: {height}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Print any errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}