using System;
using System.IO;
using Aspose.CAD;

namespace CadLoadExample
{
    class Program
    {
        static void Main()
        {
            // Placeholder path to the IGES file
            string inputPath = "input.iges";

            // Verify that the input file exists before attempting to load
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the IGES file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputPath))
                {
                    // At this point the IGES file is loaded and can be inspected or processed.
                    // Example: output basic information about the loaded image.
                    Console.WriteLine($"Loaded IGES file: {inputPath}");
                    Console.WriteLine($"Unit type: {cadImage.UnitType}");
                }
            }
            catch (Exception ex)
            {
                // Print concise error information without crashing the application
                Console.Error.WriteLine($"Error loading IGES file: {ex.Message}");
            }
        }
    }
}