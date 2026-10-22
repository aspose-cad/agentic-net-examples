using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadToPdfQualitySample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output paths
            string dataDir = @"C:\CADSamples\";
            string inputPath = Path.Combine(dataDir, "sample.dwg");
            string outputPath = Path.Combine(dataDir, "sample_output.pdf");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Configure rasterization options for higher quality rendering
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Increase line scale to improve rendering detail
                        LineScale = 2.0f,
                        // Use millimeter units for better precision
                        UnitType = UnitType.Centimenter,
                        // Set page size based on the CAD drawing dimensions
                        PageWidth = cadImage.Width,
                        PageHeight = cadImage.Height,
                        // Preserve layout scaling automatically
                        AutomaticLayoutsScaling = true
                    };

                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF file successfully created at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}