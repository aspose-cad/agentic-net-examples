using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadToPdfExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input DWG file and output PDF file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWG file using Aspose.CAD
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Create PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD image as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion successful. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}