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
            // Define input DWF file and output JPEG file paths
            string inputPath = "input.dwf";
            string outputPath = "output.jpg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWF drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure JPEG export options
                JpegOptions jpegOptions = new JpegOptions
                {
                    // Set JPEG quality (optional)
                    Quality = 90
                };

                // Save the drawing as a JPEG image
                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"DWF file successfully converted to JPEG: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}