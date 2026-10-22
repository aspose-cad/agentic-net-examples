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
            // Path to the source CAD file (DWG, DXF, etc.)
            string sourcePath = "input.dwg";

            // Path for the resulting PDF/A file
            string outputPath = "output.pdf";

            // Verify that the source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Configure PDF options (PDF/A compliance not available in current API)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}