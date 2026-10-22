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
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output.pdf");

            Image cadImage = Image.Load(inputPath);
            PdfOptions pdfOptions = new PdfOptions();

            // Configure PDF/A export settings if supported by the current API version.
            // Uncomment the appropriate line based on the available property in your Aspose.CAD version.

            // If PdfAStandard property exists:
            // pdfOptions.PdfAStandard = PdfAStandard.PdfA1b;

            // If a compliance property is used instead:
            // pdfOptions.Compliance = PdfCompliance.PdfA1b;

            cadImage.Save(outputPath, pdfOptions);
            cadImage.Dispose();

            Console.WriteLine($"PDF/A file saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}