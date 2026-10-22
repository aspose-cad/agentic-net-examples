using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Define input CAD file path
                    string inputFilePath = "sample.dwg";

                    // Verify that the input file exists
                    if (!File.Exists(inputFilePath))
                    {
                        Console.WriteLine($"Input file '{inputFilePath}' not found. Exiting.");
                        return;
                    }

                    // Create output directory
                    string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                    Directory.CreateDirectory(outputDir);

                    // Define output PDF file path
                    string outputFilePath = Path.Combine(outputDir, "sample.pdf");

                    // Load the CAD image
                    using (Image cadImage = Image.Load(inputFilePath))
                    {
                        // Set PDF save options
                        PdfOptions pdfOptions = new PdfOptions();

                        // Save the CAD drawing as PDF
                        cadImage.Save(outputFilePath, pdfOptions);
                    }

                    Console.WriteLine($"Conversion completed. PDF saved to: {outputFilePath}");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}