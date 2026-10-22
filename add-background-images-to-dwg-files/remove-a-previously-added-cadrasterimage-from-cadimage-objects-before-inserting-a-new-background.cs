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
            // Placeholder paths – replace with actual file locations.
            string inputPath = "sample.dwg";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists.
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set up PDF export options.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD drawing as a PDF file.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully converted to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
