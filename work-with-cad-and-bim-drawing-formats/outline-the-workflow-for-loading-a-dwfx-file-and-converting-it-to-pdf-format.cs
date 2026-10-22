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
            // Define input and output file paths
            string inputPath = "sample.dwf";
            string outputPath = "sample.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWFx file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Prepare PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the loaded CAD image as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred during conversion: {ex.Message}");
            }
        }
    }
}