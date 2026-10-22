using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the source CAD file (replace with an actual file path)
                string inputFilePath = "sample.dwg";

                // Define the output directory and PDF file path
                string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

                // Create the output directory if it does not exist
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Verify that the input CAD file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the drawing as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"PDF successfully saved to: {outputFilePath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}