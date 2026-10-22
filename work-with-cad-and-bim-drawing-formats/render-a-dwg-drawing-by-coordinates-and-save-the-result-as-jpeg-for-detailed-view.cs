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
            // Input DWG file path (replace with actual path)
            string inputPath = "sample.dwg";
            // Output JPEG file path
            string outputPath = "output.jpg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options.
                // Note: The SDK does not provide properties to set a specific coordinate window
                // (e.g., Center, Width, Height). Only page size and scaling can be configured.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    PageWidth = 1000,   // output image width in pixels
                    PageHeight = 1000,  // output image height in pixels
                    AutomaticLayoutsScaling = true
                };

                // JPEG export options
                JpegOptions jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered view as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"JPEG image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}