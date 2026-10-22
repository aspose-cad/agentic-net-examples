using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using System;
using System.IO;

class DwfToJpegValidator
{
    static void Main(string[] args)
    {
        // Input DWF file path
        string inputPath = @"C:\Input\sample.dwf";

        // Output JPEG file path
        string outputPath = @"C:\Output\sample.jpg";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
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
            // Load the DWF drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height,
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    }
                };

                // Configure JPEG export options
                var jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }

            // Verify the exported JPEG can be loaded and has valid dimensions
            using (Image jpegImage = Image.Load(outputPath))
            {
                // Attempt to read Width and Height via reflection (properties may exist on concrete types)
                var widthProp = jpegImage.GetType().GetProperty("Width");
                var heightProp = jpegImage.GetType().GetProperty("Height");
                int width = widthProp != null ? (int)widthProp.GetValue(jpegImage) : 0;
                int height = heightProp != null ? (int)heightProp.GetValue(jpegImage) : 0;

                if (width > 0 && height > 0)
                {
                    Console.WriteLine("JPEG export succeeded and image is valid.");
                }
                else
                {
                    Console.WriteLine("JPEG export failed: image has zero dimensions.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during conversion: {ex.Message}");
        }
    }
}