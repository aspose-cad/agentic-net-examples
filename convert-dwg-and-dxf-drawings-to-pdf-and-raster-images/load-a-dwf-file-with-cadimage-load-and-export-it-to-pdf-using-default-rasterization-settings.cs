using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadDwfToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder paths – replace with actual file locations.
                string inputPath = "sample.dwf";
                string outputPath = "output.pdf";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWF file and cast to CadImage.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Create PDF save options with default rasterization settings.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Export the CAD drawing to PDF.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF file saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}