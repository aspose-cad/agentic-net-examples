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
            string inputPath = "input.3ds";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the 3DS file
            Image cadImage = Image.Load(inputPath);

            // Prepare PDF export options
            PdfOptions pdfOptions = new PdfOptions();
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

            // Note: Aspose.CAD does not expose a DefaultLighting property in CadRasterizationOptions.
            // Therefore default lighting cannot be explicitly set via the API.

            pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Save as PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}