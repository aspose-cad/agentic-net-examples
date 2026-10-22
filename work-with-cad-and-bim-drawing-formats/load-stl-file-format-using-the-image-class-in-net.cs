using System;
using System.IO;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder path to the STL file
            string inputPath = "sample.stl";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the STL file using Aspose.CAD.Image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Example: output the unit type of the loaded CAD image
                Console.WriteLine($"Loaded STL file. Unit type: {cadImage.UnitType}");
            }
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}