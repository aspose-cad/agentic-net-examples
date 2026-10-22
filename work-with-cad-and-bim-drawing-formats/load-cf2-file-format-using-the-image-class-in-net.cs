using System;
using System.IO;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        // Path to the CF2 file (replace with actual path as needed)
        string inputPath = "sample.cf2";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CF2 file using Aspose.CAD.Image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Example operation: display the unit type of the loaded CAD image
                Console.WriteLine($"Loaded CF2 file. Unit type: {cadImage.UnitType}");
            }
        }
        catch (Exception ex)
        {
            // Print any errors that occur during loading
            Console.Error.WriteLine($"Error loading CF2 file: {ex.Message}");
        }
    }
}