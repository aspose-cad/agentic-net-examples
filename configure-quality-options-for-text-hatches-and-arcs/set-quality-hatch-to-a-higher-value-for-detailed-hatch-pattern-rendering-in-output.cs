using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string dataDir = @"C:\CAD\Samples";
            string srcFile = Path.Combine(dataDir, "sample.dxf");

            // Verify source file exists
            if (!File.Exists(srcFile))
            {
                Console.Error.WriteLine($"Source file not found: {srcFile}");
                return;
            }

            // Load the CAD document
            using (CadImage cadImage = (CadImage)Image.Load(srcFile))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High, // High-quality hatch rendering
                        Arc = RasterizationQualityValue.High
                    }
                };

                // Set up PNG export options using the rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Path to the output image
                string outFile = Path.Combine(dataDir, "output.png");

                // Ensure output directory exists
                string outDir = Path.GetDirectoryName(outFile);
                if (!string.IsNullOrEmpty(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                // Save the rendered image
                cadImage.Save(outFile, pngOptions);
                Console.WriteLine($"Image saved to: {outFile}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}