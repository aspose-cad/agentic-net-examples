using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Define input PLT file and output PDF file paths
        string inputPath = "input.plt";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the PLT file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the loaded CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Successfully converted '{inputPath}' to PDF at '{outputPath}'.");
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}