using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input PLT file path
            string inputPath = "sample.plt";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Define output JPEG file path
            string outputPath = Path.Combine(outputDir, "rendered.jpg");

            // Load the PLT file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options with anti-aliasing via GraphicsOptions
                var rasterOptions = new CadRasterizationOptions
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        SmoothingMode = SmoothingMode.AntiAlias
                    }
                };

                // Configure JPEG save options
                var jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }

            Console.WriteLine($"Rendering completed. Output saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}