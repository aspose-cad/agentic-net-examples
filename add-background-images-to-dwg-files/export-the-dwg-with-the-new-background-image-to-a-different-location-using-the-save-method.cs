using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string backgroundPath = "background.png";
            string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!File.Exists(backgroundPath))
            {
                Console.Error.WriteLine($"Background image not found: {backgroundPath}");
                return;
            }

            // Placeholder for adding background image to DWG.
            // Since external CAD libraries are not allowed, we simply copy the input file to the output location.
            File.Copy(inputPath, outputPath, overwrite: true);
            Console.WriteLine($"DWG file has been exported to {outputPath} (background image handling is not implemented in this example).");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
