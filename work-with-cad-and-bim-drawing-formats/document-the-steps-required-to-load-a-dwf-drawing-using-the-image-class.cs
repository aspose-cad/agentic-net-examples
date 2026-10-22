using System;
using System.IO;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        try
        {
            // Define the path to the DWF file
            string inputPath = "sample.dwf";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWF drawing using Aspose.CAD.Image.Load
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Output basic information about the loaded drawing
                Console.WriteLine($"Loaded DWF drawing: {Path.GetFileName(inputPath)}");
                Console.WriteLine($"Unit type: {dwfImage.UnitType}");

                // Additional processing of the drawing can be performed here
            }
        }
        catch (Exception ex)
        {
            // Print any errors that occur during loading
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}