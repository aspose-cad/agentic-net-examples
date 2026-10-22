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
            // Define input CAD file path (placeholder)
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDirectory = "output";
            Directory.CreateDirectory(outputDirectory);
            string outputPath = Path.Combine(outputDirectory, "output.pdf");

            // Load the CAD image using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PDF save options
                PdfOptions pdfOptions = new PdfOptions();
                // Example: set PDF compliance level if needed
                // pdfOptions.Compliance = PdfCompliance.PdfA1b;

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Conversion completed successfully. Output file: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}