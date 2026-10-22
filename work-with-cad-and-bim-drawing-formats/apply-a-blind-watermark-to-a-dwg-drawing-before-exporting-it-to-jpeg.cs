using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

namespace BlindWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths for input DWG and output JPEG
            string sourcePath = "input.dwg";
            string outputPath = "output.jpg";

            // Verify input file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Input file not found: {sourcePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the DWG drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD‑specific features
                    CadImage cadImage = (CadImage)image;

                    // Set the blind watermark text to embed
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Check embedding status before saving
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    Console.WriteLine($"Embedding status: {embedStatus}");
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Embedding failed. The saved image may not contain a valid blind watermark.");
                    }

                    // Save the drawing as JPEG
                    JpegOptions jpegOptions = new JpegOptions();
                    cadImage.Save(outputPath, jpegOptions);
                }

                // Note: Blind watermark validation is not supported on raster formats such as JPEG.
                Console.WriteLine("DWG processed and saved as JPEG with blind watermark embedded (validation not applicable for JPEG).");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}