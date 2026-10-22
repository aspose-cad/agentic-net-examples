using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input CAD file path (replace with actual file)
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Output PDF file path
        string outputPath = "output.pdf";

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,   // optional: set desired page width
                    PageHeight = 600   // optional: set desired page height
                };

                // Assign rasterization options to the PDF options
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD drawing as PDF with error handling
                try
                {
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Successfully exported to PDF: {outputPath}");
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"Export error: {exportEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}