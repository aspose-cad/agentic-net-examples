using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputFilePath = "input.dwg";
        if (!File.Exists(inputFilePath))
        {
            Console.Error.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        string outputDirectory = "output";
        Directory.CreateDirectory(outputDirectory);
        string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

        try
        {
            using (Image cadImage = Image.Load(inputFilePath))
            {
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputFilePath, pdfOptions);
            }

            Console.WriteLine($"Conversion completed. Output saved to {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Conversion error: {ex.Message}");
        }
    }
}