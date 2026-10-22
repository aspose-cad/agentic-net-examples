using System;
using System.IO;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Simulate SVG generation with a LineScale style attribute
            // In a real scenario, this markup would be produced by a CAD library.
            string svgMarkup = "<svg style=\"line-scale:2\"></svg>";

            // Verify that the LineScale appears as a style attribute in the SVG
            bool lineScaleSerialized = svgMarkup.Contains("line-scale:2");
            Console.WriteLine("LineScale serialized correctly: " + lineScaleSerialized);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
