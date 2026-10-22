using System;
using System.IO;
using Aspose.CAD;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder path to the DWF file
            string inputFilePath = "input.dwf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found - {inputFilePath}");
                    return;
                }

                // Load the DWF drawing using the standard Aspose.CAD loader
                using (Image dwfImage = Image.Load(inputFilePath))
                {
                    // Output basic information about the loaded drawing
                    Console.WriteLine($"Successfully loaded DWF file: {inputFilePath}");
                    Console.WriteLine($"Image size: {dwfImage.Width} x {dwfImage.Height}");
                    // FileFormat property is not available; display the image type instead
                    Console.WriteLine($"Image type: {dwfImage.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                // Print concise error information without crashing
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}