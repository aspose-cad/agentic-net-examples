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
            // Input DWFx file path
            string inputPath = "sample.dwf";
            // Output PDF file path
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWFx file using Aspose.CAD
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Export the loaded DWFx to PDF
                dwfImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Conversion successful: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}