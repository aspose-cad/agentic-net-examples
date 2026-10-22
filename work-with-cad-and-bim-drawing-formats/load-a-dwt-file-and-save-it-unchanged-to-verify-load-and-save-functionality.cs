using System;
using System.IO;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string inputPath = "input.dwt";
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "output.dwt");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the DWT file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Save the image unchanged to the output path
                cadImage.Save(outputPath);
            }

            Console.WriteLine($"File saved successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}