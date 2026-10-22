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
            string inputPath = "input.dwg";
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "output.pdf");

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            Image cadImage = Image.Load(inputPath);
            PdfOptions pdfOptions = new PdfOptions();
            // Note: PDF/A compliance can be set via PdfOptions if supported by the library version.
            cadImage.Save(outputPath, pdfOptions);
            cadImage.Dispose();

            Console.WriteLine($"PDF file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}