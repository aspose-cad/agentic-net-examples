using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.drc";
        string outputPath = "output.jpg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DRC drawing
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,   // Desired output width in pixels
                PageHeight = 1200,  // Desired output height in pixels
                UnitType = UnitType.Centimenter,
                LineScale = 1.0f
                // NOTE: The current Aspose.CAD SDK does not provide a Viewport property
                // on CadRasterizationOptions, so rendering a specific viewport directly
                // is not supported. To obtain a viewport you would need to post‑process
                // the rendered bitmap (e.g., crop), which requires additional libraries
                // not permitted in this console‑only sample.
            };

            // Set JPEG export options and attach rasterization settings
            JpegOptions jpegOptions = new JpegOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rendered image
            cadImage.Save(outputPath, jpegOptions);
            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}