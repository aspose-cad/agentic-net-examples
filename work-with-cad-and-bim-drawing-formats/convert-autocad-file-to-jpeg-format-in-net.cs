using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Define input and output file paths
        string inputFilePath = "input.dwg";
        string outputDirectory = "output";
        string outputFilePath = Path.Combine(outputDirectory, "output.jpg");

        try
        {
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

            // Load the CAD file
            using (Image cadImage = Image.Load(inputFilePath))
            {
                // Set JPEG save options (default options are sufficient for basic conversion)
                JpegOptions jpegOptions = new JpegOptions();

                // Save the CAD image as JPEG
                cadImage.Save(outputFilePath, jpegOptions);
            }

            Console.WriteLine($"Conversion successful. JPEG saved to: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}