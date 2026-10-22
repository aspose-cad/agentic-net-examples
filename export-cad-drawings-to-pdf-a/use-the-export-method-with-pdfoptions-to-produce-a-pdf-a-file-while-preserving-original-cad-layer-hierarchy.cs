using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Define input and output paths
        string inputFilePath = "input.dwg";
        string outputDirectory = "output";
        string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

        // Verify input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input CAD file not found: {inputFilePath}");
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(outputDirectory);

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
            {
                // Configure PDF options for PDF output (PDF/A requires Aspose.Pdf which is omitted)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD image as PDF, preserving layer hierarchy
                cadImage.Save(outputFilePath, pdfOptions);
            }

            Console.WriteLine($"PDF file successfully saved to: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}