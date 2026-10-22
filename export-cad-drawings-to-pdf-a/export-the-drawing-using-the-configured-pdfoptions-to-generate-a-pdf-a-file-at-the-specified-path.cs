using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            Image cadImage = Image.Load(inputPath);

            PdfOptions pdfOptions = new PdfOptions();
            // Note: PDF/A compliance setting is not available in the current API version.
            // The generated PDF will be a standard PDF. Adjust the options as needed when the API supports PDF/A.

            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"PDF file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}