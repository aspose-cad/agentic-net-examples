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
            // Placeholder paths – replace with actual file locations
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully exported '{inputPath}' to PDF at '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during CAD export: {ex.Message}");
            }
        }
    }
}