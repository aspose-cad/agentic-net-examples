using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadDwfLoadExample
{
    class Program
    {
        static void Main()
        {
            // Placeholder path to the DWF file.
            string inputPath = "sample.dwf";

            // Verify that the input file exists.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the DWF file using Aspose.CAD.
                using (Image dwfImage = Image.Load(inputPath))
                {
                    // If loading succeeds, optionally perform further operations.
                    Console.WriteLine("DWF file loaded successfully.");

                    // Example: export the loaded DWF to PDF.
                    string outputPath = "output.pdf";
                    PdfOptions pdfOptions = new PdfOptions();
                    dwfImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Exported to PDF: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                // Log error details without crashing the application.
                Console.Error.WriteLine("Error loading DWF file:");
                Console.Error.WriteLine($"Message: {ex.Message}");
                Console.Error.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}