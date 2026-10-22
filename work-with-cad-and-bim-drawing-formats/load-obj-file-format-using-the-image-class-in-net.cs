using System;
using System.IO;
using Aspose.CAD;

namespace AsposeCadExample
{
    class Program
    {
        static void Main()
        {
            // Placeholder path to the OBJ file
            string inputPath = "sample.obj";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the OBJ file using Aspose.CAD.Image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Example operation: output the unit type of the loaded CAD image
                    Console.WriteLine($"Loaded OBJ file. Unit type: {cadImage.UnitType}");
                }
            }
            catch (Exception ex)
            {
                // Print any errors that occur during loading
                Console.Error.WriteLine($"Error loading OBJ file: {ex.Message}");
            }
        }
    }
}