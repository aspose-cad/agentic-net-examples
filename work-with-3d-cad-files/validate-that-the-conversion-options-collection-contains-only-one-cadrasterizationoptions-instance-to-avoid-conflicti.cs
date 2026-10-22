using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadConversionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (replace with actual path)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output PDF file path
                string outputPath = "output.pdf";

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Create PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Configure rasterization options (only one instance)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600
                        // No Layout property; defaults will be used
                    };

                    // Assign the rasterization options to the PDF options
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Validation: ensure only one CadRasterizationOptions instance is present
                    if (pdfOptions.VectorRasterizationOptions == null)
                    {
                        throw new InvalidOperationException("CadRasterizationOptions instance is missing.");
                    }

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}