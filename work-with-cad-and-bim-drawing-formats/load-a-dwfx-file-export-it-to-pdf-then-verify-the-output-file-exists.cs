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
                // Define input and output paths
                string inputFilePath = "input.dwf";
                string outputDirectory = "Output";
                string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the DWFx file
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                // Verify that the PDF was created
                if (File.Exists(outputFilePath))
                {
                    Console.WriteLine($"PDF successfully created at: {outputFilePath}");
                }
                else
                {
                    Console.Error.WriteLine("Failed to create the PDF output file.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}