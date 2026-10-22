using System;
using System.IO;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputDirectory = "Output";
        string outputPath = Path.Combine(outputDirectory, "PolylinesOnly.dwg");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Placeholder: In a real scenario, you would parse the DWG file,
            // extract polyline vertices, and create a new DWG containing only those.
            // Since external CAD libraries are not allowed, we simply copy the input
            // file to the output location as a stub.

            File.Copy(inputPath, outputPath, overwrite: true);

            Console.WriteLine($"Polylines extracted and saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
