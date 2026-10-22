using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadDwfToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations
            string inputPath = "sample.dwf";
            string outputPath = "sample.pdf";

            try
            {
                // Verify that the source DWF file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWF drawing
                using (Image dwfImage = Image.Load(inputPath))
                {
                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the drawing as PDF
                    dwfImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully converted '{inputPath}' to PDF at '{outputPath}'.");
            }
            catch (Exception ex)
            {
                // Output concise error information
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}