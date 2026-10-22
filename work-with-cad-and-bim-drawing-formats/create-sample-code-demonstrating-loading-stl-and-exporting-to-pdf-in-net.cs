using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input STL file and output PDF file paths
            string inputPath = "sample.stl";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the STL file using Aspose.CAD.Image.Load
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the loaded CAD image as a PDF file
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}