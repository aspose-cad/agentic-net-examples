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
            // Define input OBJ file and output PDF path
            string inputPath = "input.obj";
            string outputDirectory = "output";
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

            // Load the OBJ file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the loaded CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF file saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}