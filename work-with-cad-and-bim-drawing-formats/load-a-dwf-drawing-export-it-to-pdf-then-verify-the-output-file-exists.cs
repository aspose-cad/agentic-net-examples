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
            // Define input and output paths
            string inputPath = "Resources/sample.dwf";
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "sample.pdf");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the DWF drawing
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save as PDF
                dwfImage.Save(outputPath, pdfOptions);
            }

            // Verify the output file was created
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"PDF file successfully created at: {outputPath}");
            }
            else
            {
                Console.Error.WriteLine("Failed to create the PDF file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}