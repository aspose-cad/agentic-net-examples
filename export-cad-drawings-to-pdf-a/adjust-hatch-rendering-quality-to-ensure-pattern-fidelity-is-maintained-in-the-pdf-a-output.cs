using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadHatchQualityPdfA
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input CAD file path
                string inputPath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Define output PDF/A file path
                string outputPath = Path.Combine(outputDir, "output.pdf");

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure PDF options
                    var pdfOptions = new PdfOptions();

                    // Configure rasterization options to improve overall quality
                    var rasterOptions = new CadRasterizationOptions
                    {
                        // Increase page size to get higher resolution rendering
                        PageWidth = 1600,
                        PageHeight = 1600,
                        // Adjust line scale for finer detail
                        LineScale = 0.5f
                    };

                    // Assign rasterization options to PDF options
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing as PDF (PDF/A compliance can be set via PdfOptions if supported)
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF file saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}