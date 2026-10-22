using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadIgesToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input IGES file and output PDF file paths.
            string inputPath = "input.iges";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists.
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Error: Input file not found: {inputPath}");
                    return;
                }

                // Load the IGES model using Aspose.CAD.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Prepare PDF export options.
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        // Use default vector rasterization options to preserve dimensional accuracy.
                        VectorRasterizationOptions = new CadRasterizationOptions()
                    };

                    // Save the loaded CAD image as a PDF document.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully converted IGES to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                // Output concise error information.
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
    }
}