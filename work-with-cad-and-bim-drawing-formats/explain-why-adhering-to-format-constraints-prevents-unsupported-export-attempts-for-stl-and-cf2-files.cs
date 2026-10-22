using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.pdf");

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            Directory.CreateDirectory(outputDir);

            using (Image cadImage = Image.Load(inputPath))
            {
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("Conversion succeeded.");
            Console.WriteLine("Adhering to format constraints prevents attempts to export to unsupported formats such as STL or CF2, because Aspose.CAD provides ImageOptions only for supported target formats. Using unsupported formats would result in missing option classes and compile‑time errors.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}