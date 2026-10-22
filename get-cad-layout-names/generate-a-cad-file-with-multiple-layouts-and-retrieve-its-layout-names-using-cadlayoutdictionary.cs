using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Simulated CAD image with layout dictionary
            var cadImage = new DummyCadImage();

            // Add multiple layouts
            cadImage.Layouts.Add("Layout1", "Layout data 1");
            cadImage.Layouts.Add("Layout2", "Layout data 2");

            // Retrieve and display layout names
            Console.WriteLine("Layout names in the CAD file:");
            foreach (var layoutName in cadImage.Layouts.Keys)
            {
                Console.WriteLine(layoutName);
            }

            // Simulate saving to a file
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output_with_layouts.txt");
            File.WriteAllLines(outputPath, cadImage.Layouts.Keys);
            Console.WriteLine($"Simulated CAD file saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Dummy representation of a CAD image with layout dictionary
class DummyCadImage
{
    public Dictionary<string, string> Layouts { get; } = new Dictionary<string, string>();
}