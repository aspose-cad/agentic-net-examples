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
            // Define input COLLADA file path (replace with actual path)
            string inputPath = "input.dae";

            // Define output directory and PDF file path
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "output.pdf");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the COLLADA drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}