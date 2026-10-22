using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input COLLADA file and output PDF file paths
                string inputPath = "input.dae";
                string outputPath = "output.pdf";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the COLLADA drawing using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Export the CAD drawing directly to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully exported '{inputPath}' to PDF as '{outputPath}'.");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}