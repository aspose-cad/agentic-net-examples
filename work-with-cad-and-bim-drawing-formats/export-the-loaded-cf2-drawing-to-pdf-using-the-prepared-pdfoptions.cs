using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations.
            string inputPath = "sample.cf2";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists.
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CF2 drawing.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Prepare PDF export options.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Optional: control rasterization settings.
                    // pdfOptions.VectorRasterizationOptions = new VectorRasterizationOptions();

                    // Save the drawing as PDF.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully exported PDF to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}