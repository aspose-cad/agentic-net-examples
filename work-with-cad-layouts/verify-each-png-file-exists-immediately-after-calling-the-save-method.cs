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
                // Define input CAD file path
                string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                string inputFilePath = Path.Combine(dataDirectory, "sample.dwg");

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine($"Input CAD file not found: {inputFilePath}");
                    return;
                }

                // Create output directory
                string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDirectory);
                string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

                // Load CAD image
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Set PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                // Verify output file was created
                if (File.Exists(outputFilePath))
                {
                    Console.WriteLine($"PDF file saved successfully: {outputFilePath}");
                }
                else
                {
                    Console.WriteLine("Failed to save PDF file.");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}