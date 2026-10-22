using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input DWG file and output PDF location
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.pdf");

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Conversion completed. PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}