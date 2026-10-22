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
            // Define input DWF file and output PDF file paths
            string inputPath = "input.dwf";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWF drawing using Aspose.CAD.Image.Load
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Create PDF export options (default settings)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the drawing as PDF
                dwfImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF file saved to {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}