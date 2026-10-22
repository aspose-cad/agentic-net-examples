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
            string inputPath = "sample.dwf";
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "sample.jpg");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Load the DWF drawing
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Set JPEG export options
                JpegOptions jpegOptions = new JpegOptions();

                // Save the drawing as JPEG
                dwfImage.Save(outputPath, jpegOptions);
            }

            // Verify that the output file was created
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"JPEG file successfully created at: {outputPath}");
            }
            else
            {
                Console.Error.WriteLine("Failed to create the JPEG output file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}