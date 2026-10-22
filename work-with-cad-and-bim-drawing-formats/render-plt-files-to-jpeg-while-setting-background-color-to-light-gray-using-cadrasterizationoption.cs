using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class PltToJpegConverter
{
    static void Main(string[] args)
    {
        // Input PLT file path
        string inputPath = @"Samples/Example.plt";
        // Output directory
        string outputDir = @"Output";

        // Validate input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the PLT file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    // Set background to light gray (using Aspose.CAD's Color if available)
                    BackgroundColor = Color.LightGray,
                    // Preserve the original size
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height,
                    // Optional: set resolution if needed
                    // HorizontalResolution = 300,
                    // VerticalResolution = 300
                };

                // Set JPEG export options
                var jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Build output file path
                string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".jpg");

                // Save the rendered image
                cadImage.Save(outputPath, jpegOptions);

                Console.WriteLine($"Successfully converted PLT to JPEG: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}