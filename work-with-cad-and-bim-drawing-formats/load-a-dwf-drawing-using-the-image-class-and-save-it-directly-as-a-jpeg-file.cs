using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadDwfToJpeg
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder file paths
            string inputPath = "input.dwf";
            string outputPath = "output.jpg";

            try
            {
                // Verify that the input DWF file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWF drawing using Aspose.CAD.Image
                using (Image dwfImage = Image.Load(inputPath))
                {
                    // Set JPEG save options
                    JpegOptions jpegOptions = new JpegOptions();

                    // Save the drawing directly as a JPEG file
                    dwfImage.Save(outputPath, jpegOptions);
                }

                Console.WriteLine($"Successfully converted '{inputPath}' to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                // Print any errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}