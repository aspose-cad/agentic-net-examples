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
            // Define input and output file paths
            string inputPath = "input.dwf";
            string outputPath = "output.jpg";

            // Verify that the input DWF file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWF drawing using Aspose.CAD.Image
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Configure JPEG export options
                JpegOptions jpegOptions = new JpegOptions
                {
                    // Optional: set JPEG quality (0-100)
                    Quality = 90
                };

                // Save the drawing as a JPEG image
                dwfImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"DWF file successfully saved as JPEG to: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}