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
            string inputFilePath = "input.dwg";
            string outputFilePath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Error: Input file not found at '{inputFilePath}'.");
                return;
            }

            try
            {
                // Load the CAD image using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Create PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD image as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"Successfully converted '{inputFilePath}' to PDF at '{outputFilePath}'.");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}