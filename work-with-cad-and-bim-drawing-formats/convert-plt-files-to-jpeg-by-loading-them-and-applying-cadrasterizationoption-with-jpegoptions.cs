using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class PltToJpegConverter
{
    static void Main(string[] args)
    {
        // Expect input PLT path and output JPEG path as arguments
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: PltToJpegConverter <input.plt> <output.jpg>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        // Validate input file existence
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Error: Input file '{inputPath}' does not exist.");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the PLT file
            Image cadImage = Image.Load(inputPath);

            // Prepare rasterization options
            var rasterOptions = new CadRasterizationOptions
            {
                // Use the original image dimensions for the raster size
                PageWidth = cadImage.Width,
                PageHeight = cadImage.Height,
                // Set a white background (Aspose.CAD.ImageOptions.Color, not System.Drawing.Color)
                BackgroundColor = Color.White,
                // High quality for text, hatch and arc rendering
                Quality = new RasterizationQuality
                {
                    Text = RasterizationQualityValue.High,
                    Hatch = RasterizationQualityValue.High,
                    Arc = RasterizationQualityValue.High
                }
            };

            // Prepare JPEG export options
            var jpegOptions = new JpegOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rasterized image as JPEG
            cadImage.Save(outputPath, jpegOptions);

            Console.WriteLine($"Conversion successful. JPEG saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during conversion: {ex.Message}");
        }
    }
}