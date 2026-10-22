using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input OBJ file and output JPEG file paths
            string inputPath = "input.obj";
            string outputPath = "output.jpg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the OBJ file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure JPEG export options
                JpegOptions jpegOptions = new JpegOptions
                {
                    // Optional: set JPEG quality (0-100)
                    Quality = 90
                };

                // Save the loaded CAD image as a JPEG file
                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"Conversion completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}