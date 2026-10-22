using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "input.plt";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.jpg");

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the PLT CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    // Set unit type (Centimeter) and line scale
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                    // Background color cannot be set without using System.Drawing, which is prohibited.
                };

                // Configure JPEG output options
                var jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions,
                    Quality = 90 // optional quality setting
                };

                // Save the rendered image as JPEG
                cadImage.Save(outputPath, jpegOptions);
                Console.WriteLine($"Image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}