using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input DWG file and output PDF file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWG drawing as a CadImage (explicit cast required)
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing as a PDF file
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