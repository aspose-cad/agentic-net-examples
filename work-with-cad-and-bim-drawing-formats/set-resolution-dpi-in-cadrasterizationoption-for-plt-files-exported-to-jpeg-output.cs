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
            // Input CAD file path
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Output JPEG file path
            string outputPath = "output.jpg";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Desired DPI for the rasterized image
            const float dpi = 300f;
            // Page size in inches (e.g., standard Letter size)
            const float pageWidthInches = 8.5f;
            const float pageHeightInches = 11f;

            // Configure rasterization options to emulate the required DPI
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = pageWidthInches * dpi,   // width in pixels
                PageHeight = pageHeightInches * dpi // height in pixels
            };

            // Set up JPEG save options and attach the rasterization settings
            JpegOptions jpegOptions = new JpegOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Load the CAD image, rasterize and save as JPEG
            Image cadImage = Image.Load(inputPath);
            cadImage.Save(outputPath, jpegOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}