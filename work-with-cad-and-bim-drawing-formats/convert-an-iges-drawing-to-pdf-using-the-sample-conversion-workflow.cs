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
            string inputPath = "sample.iges";
            string outputPath = "sample.pdf";

            try
            {
                // Verify that the input IGES file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the IGES drawing using Aspose.CAD
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion successful. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}