using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Define input OBJ file and output PDF file paths
        string inputPath = "input.obj";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the OBJ file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD image as PDF
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