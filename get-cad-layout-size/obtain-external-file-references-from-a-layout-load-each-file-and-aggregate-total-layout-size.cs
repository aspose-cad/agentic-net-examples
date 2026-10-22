using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the main CAD file
            string mainCadPath = "input.dwg";

            // Verify that the file exists
            if (!File.Exists(mainCadPath))
            {
                Console.Error.WriteLine($"File not found: {mainCadPath}");
                return;
            }

            // Variable to hold the aggregated layout area (in square units)
            double totalArea = 0;

            // Load the main CAD file
            using (Image mainImage = Image.Load(mainCadPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)mainImage;

                // Iterate through each layout in the CAD file
                foreach (var layoutEntry in cadImage.Layouts)
                {
                    CadLayout layout = layoutEntry.Value;

                    // Calculate width and height from extents
                    double width = Math.Abs(layout.MaxExtents.X - layout.MinExtents.X);
                    double height = Math.Abs(layout.MaxExtents.Y - layout.MinExtents.Y);

                    // Accumulate area
                    totalArea += width * height;
                }
            }

            // Output the aggregated total layout area
            Console.WriteLine($"Total aggregated layout area: {totalArea} square units");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}