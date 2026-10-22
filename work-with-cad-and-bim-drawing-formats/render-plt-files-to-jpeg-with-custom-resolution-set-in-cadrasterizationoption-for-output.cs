using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string sourcePath = "input.plt";
        string outputPath = "output.jpg";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image cadImage = Image.Load(sourcePath))
            {
                // Configure JPEG output options
                JpegOptions jpegOptions = new JpegOptions();

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // The SDK does not expose HorizontalResolution/VerticalResolution properties.
                    // Use UnitType and LineScale to affect the rendered size.
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    PageWidth = 1600,   // width in points (1/72 inch)
                    PageHeight = 1200   // height in points
                };

                jpegOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, jpegOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}