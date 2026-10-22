using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadDwfToJpeg
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations.
            string inputPath = "sample.dwf";
            string outputPath = "output.jpg";

            try
            {
                // Verify that the input DWF file exists.
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWF drawing using Aspose.CAD.Image.
                using (Image dwfImage = Image.Load(inputPath))
                {
                    // Configure JPEG export options.
                    JpegOptions jpegOptions = new JpegOptions
                    {
                        // Optional: set JPEG quality (0-100). Default is 75.
                        Quality = 90
                    };

                    // Save the drawing as a JPEG file.
                    dwfImage.Save(outputPath, jpegOptions);
                }

                Console.WriteLine($"Successfully converted '{inputPath}' to JPEG at '{outputPath}'.");
            }
            catch (Exception ex)
            {
                // Print concise error information.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}