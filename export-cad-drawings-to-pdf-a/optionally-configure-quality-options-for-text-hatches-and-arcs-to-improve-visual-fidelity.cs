using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                    // Additional rasterization settings can be configured here if needed
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}