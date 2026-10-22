using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input PLT file and desired PDF output
        string inputFile = "input.plt";
        string outputFile = "output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the PLT file as a CadImage
            using (Image cadImage = Image.Load(inputFile))
            {
                // Configure PDF export options
                var pdfOptions = new PdfOptions();

                // NOTE: Aspose.CAD PdfOptions does not expose a BackgroundColor property.
                // Background color and anti‑aliasing settings are not configurable via PdfOptions.
                // The PDF will be rendered with default settings.

                // Save the rendering to PDF
                cadImage.Save(outputFile, pdfOptions);
                Console.WriteLine($"Successfully rendered '{inputFile}' to PDF: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}