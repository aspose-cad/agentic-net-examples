using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadPltToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input PLT file path and output PDF file path
            string inputPath = "sample.plt";
            string outputPath = "result.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the PLT file
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Set rasterization options for vector rendering
                    pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        // Example page size; adjust as needed
                        PageWidth = 800,
                        PageHeight = 600
                        // Additional rasterization settings can be configured here
                    };

                    // Save the loaded CAD image as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}