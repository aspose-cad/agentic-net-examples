using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input IGES file path and output PDF file path
            string inputFilePath = "input.iges";
            string outputDirectory = "output";
            string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the IGES file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Create PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the loaded CAD image as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"Successfully exported IGES to PDF: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}