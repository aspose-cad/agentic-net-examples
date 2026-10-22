using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadToPdfExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations.
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing.
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Set the default font to ensure proper text rendering.
                    cadImage.DefaultFont = "Arial";

                    // Prepare PDF export options.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the drawing as PDF.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion successful. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}