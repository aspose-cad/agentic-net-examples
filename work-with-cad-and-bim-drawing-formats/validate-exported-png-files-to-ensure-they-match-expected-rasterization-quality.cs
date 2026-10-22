using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPngValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path
                string cadFilePath = "sample.dwg";
                // Output PNG file path
                string pngFilePath = "sample_high_quality.png";

                // Verify input CAD file exists
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"Input CAD file not found: {cadFilePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(pngFilePath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Configure rasterization options with high quality for text, hatch and arc
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    },
                    PageWidth = 1600,
                    PageHeight = 1200,
                    UnitType = UnitType.Millimeter
                };

                // Configure PNG export options and assign the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Load the CAD image, render and save as PNG with the specified quality
                using (Image cadImage = Image.Load(cadFilePath))
                {
                    cadImage.Save(pngFilePath, pngOptions);
                }

                // Simple validation: check that the PNG file was created
                if (File.Exists(pngFilePath))
                {
                    Console.WriteLine("PNG file exported successfully with high rasterization quality.");
                }
                else
                {
                    Console.WriteLine("Failed to export PNG file.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}