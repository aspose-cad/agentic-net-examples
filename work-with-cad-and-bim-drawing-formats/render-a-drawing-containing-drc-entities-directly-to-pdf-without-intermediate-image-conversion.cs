using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        // Path to the source CAD file
        string sourcePath = "input.dwg";
        // Path to the output PDF file
        string outputPath = "output.pdf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options for vector output
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the drawing directly to PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}