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
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Optional rasterization settings (uncomment to customize)
                // pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions
                // {
                //     PageWidth = 1600,
                //     PageHeight = 1200,
                //     Layouts = new string[] { "Model" }
                // };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD file saved as PDF to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}