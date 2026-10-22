using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output paths
            string inputFilePath = "input.dwg";
            string outputDirectory = "Output";
            string outputFilePath = Path.Combine(outputDirectory, "output.dwg");

            // Verify that the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the DWG file
            using (Image cadImage = Image.Load(inputFilePath))
            {
                // Prepare DWG save options
                DwgOptions dwgOptions = new DwgOptions();

                // Save the image as DWG, preserving embedded data
                cadImage.Save(outputFilePath, dwgOptions);
            }

            Console.WriteLine($"File successfully saved to: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}