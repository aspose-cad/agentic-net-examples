using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
        string outputPath = Path.Combine(outputDir, "Result.pdf");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            Image cadImage = Image.Load(inputPath);
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1200,
                PageHeight = 800
                // Note: Aspose.CAD does not expose a direct Margin property.
                // Margins can be simulated by adjusting PageWidth/PageHeight as needed.
            };

            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            cadImage.Save(outputPath, pdfOptions);
            cadImage.Dispose();

            Console.WriteLine($"PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
