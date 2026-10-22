using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputFile = "sample.3dxml";
        string outputDir = "Output";

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        try
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        string outputFile = Path.Combine(outputDir, "result.pdf");

        try
        {
            using (Image cadImage = Image.Load(inputFile))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 8.5f,   // Letter width in inches
                    PageHeight = 11f    // Letter height in inches
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputFile, pdfOptions);
            }

            Console.WriteLine($"PDF saved to {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing or saving: {ex.Message}");
        }
    }
}
