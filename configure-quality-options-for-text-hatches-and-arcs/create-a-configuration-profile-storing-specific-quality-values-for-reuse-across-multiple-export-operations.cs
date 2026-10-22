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
            string inputPath = "sample.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Create a configuration profile with quality settings
            ExportQualityProfile qualityProfile = new ExportQualityProfile
            {
                JpegQuality = 90,
                PngCompressionLevel = 6
            };

            // Load CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export to JPEG using profile
                string jpegPath = "output.jpg";
                JpegOptions jpegOptions = new JpegOptions
                {
                    Quality = qualityProfile.JpegQuality
                };
                cadImage.Save(jpegPath, jpegOptions);

                // Export to PNG using profile
                string pngPath = "output.png";
                PngOptions pngOptions = new PngOptions
                {
                    CompressionLevel = qualityProfile.PngCompressionLevel
                };
                cadImage.Save(pngPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Configuration profile class
public class ExportQualityProfile
{
    public int JpegQuality { get; set; }
    public int PngCompressionLevel { get; set; }
}