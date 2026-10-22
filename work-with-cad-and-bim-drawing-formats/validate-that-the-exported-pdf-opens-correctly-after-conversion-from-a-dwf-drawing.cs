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
            string inputPath = "sample.dwf";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            if (File.Exists(outputPath) && new FileInfo(outputPath).Length > 0)
            {
                Console.WriteLine("PDF exported successfully and appears valid.");
            }
            else
            {
                Console.Error.WriteLine("PDF export failed or the file is empty.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}