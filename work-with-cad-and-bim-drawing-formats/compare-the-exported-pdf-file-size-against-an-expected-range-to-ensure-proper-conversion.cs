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
                // Path to the source CAD file
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path where the PDF will be saved
                string outputPath = "output.pdf";

                // Expected PDF file size range (in bytes)
                long expectedMinSize = 50000;   // 50 KB
                long expectedMaxSize = 500000;  // 500 KB

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Export the drawing to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                // Retrieve the size of the generated PDF file
                FileInfo pdfInfo = new FileInfo(outputPath);
                long actualSize = pdfInfo.Length;

                // Verify that the file size falls within the expected range
                if (actualSize >= expectedMinSize && actualSize <= expectedMaxSize)
                {
                    Console.WriteLine($"PDF file size {actualSize} bytes is within the expected range.");
                }
                else
                {
                    Console.WriteLine($"PDF file size {actualSize} bytes is outside the expected range ({expectedMinSize}-{expectedMaxSize} bytes).");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}