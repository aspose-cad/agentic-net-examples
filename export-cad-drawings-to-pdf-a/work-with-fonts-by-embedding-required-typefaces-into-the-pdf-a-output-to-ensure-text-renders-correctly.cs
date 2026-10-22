using Aspose.CAD.FileFormats.Cad;
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
            // Input CAD file path
            string inputFile = "sample.dwg";
            // Desired PDF output path
            string outputFile = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputFile))
                {
                    // Set default font for text rendering
                    cadImage.DefaultFont = "Arial";

                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save as PDF
                    cadImage.Save(outputFile, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully converted to PDF: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during conversion: {ex.Message}");
            }
        }
    }
}