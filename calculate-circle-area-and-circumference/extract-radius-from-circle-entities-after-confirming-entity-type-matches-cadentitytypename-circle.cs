using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            // Verify that the file exists before attempting to load
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast the generic Image to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                bool circleFound = false;

                // Iterate over top‑level entities in the drawing
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Identify circle entities
                    if (entity is CadCircle circle)
                    {
                        circleFound = true;

                        // Retrieve radius (Aspose.CAD provides a Radius property)
                        double radius = circle.Radius;

                        // Calculate geometric properties
                        double area = Math.PI * radius * radius;
                        double circumference = 2 * Math.PI * radius;

                        // Output results
                        Console.WriteLine($"Circle found in layer '{circle.LayerName}':");
                        Console.WriteLine($"  Center: ({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z})");
                        Console.WriteLine($"  Radius: {radius}");
                        Console.WriteLine($"  Area: {area}");
                        Console.WriteLine($"  Circumference: {circumference}");
                    }
                }

                if (!circleFound)
                {
                    Console.WriteLine("No circles were found in the drawing.");
                }
            }
        }
        catch (Exception ex)
        {
            // Write a concise error message and exit gracefully
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}