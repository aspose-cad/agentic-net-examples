using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the source CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare PDF export options (default settings)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}