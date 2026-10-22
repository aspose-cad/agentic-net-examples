using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file
            string sourcePath = "sample.dwg";
            // Path for the output JPEG file
            string outputPath = "output.jpg";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Use default UnitType (no explicit setting)
                        AutomaticLayoutsScaling = true,
                        // Optional: set page size in millimeters (A4) – the renderer will scale accordingly
                        PageWidth = 210,
                        PageHeight = 297,
                        Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                    };

                    // Set up JPEG save options and attach rasterization settings
                    JpegOptions jpegOptions = new JpegOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing as JPEG
                    cadImage.Save(outputPath, jpegOptions);
                }

                Console.WriteLine("CAD file has been converted to JPEG successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}