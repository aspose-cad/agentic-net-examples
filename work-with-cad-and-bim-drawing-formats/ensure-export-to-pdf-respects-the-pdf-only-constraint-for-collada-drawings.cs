using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dae";
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
                PdfOptions pdfOptions = new PdfOptions();

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1200;
                rasterOptions.PageHeight = 800;
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF exported successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}