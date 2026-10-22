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
            // Define input and output file paths
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

                // Load the CAD file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully exported PDF to: {outputPath}");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}