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

            using (Image cadImage = Image.Load(inputPath))
            {
                // Save as PDF
                string pdfPath = Path.Combine(outputDir, "output.pdf");
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);

                // Save as PNG with rasterization options
                string pngPath = Path.Combine(outputDir, "output.png");
                PngOptions pngOptions = new PngOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };
                pngOptions.VectorRasterizationOptions = rasterOptions;
                cadImage.Save(pngPath, pngOptions);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}