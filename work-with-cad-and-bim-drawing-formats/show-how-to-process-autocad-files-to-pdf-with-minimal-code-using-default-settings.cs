using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input DWG/DXF file and output PDF file paths
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the source file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create PDF export options (default settings)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}