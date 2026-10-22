using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Define input STL file path and output PDF file path
        string inputFilePath = "input.stl";
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

            // Load the STL file using Aspose.CAD
            using (Image cadImage = Image.Load(inputFilePath))
            {
                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the loaded CAD image as PDF
                cadImage.Save(outputFilePath, pdfOptions);
            }

            Console.WriteLine($"Successfully exported STL to PDF: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}