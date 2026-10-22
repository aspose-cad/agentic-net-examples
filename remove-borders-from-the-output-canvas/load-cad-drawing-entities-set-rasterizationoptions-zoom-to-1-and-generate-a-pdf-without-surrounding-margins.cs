using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                var rasterOptions = new CadRasterizationOptions
                {
                    Zoom = 1,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 },
                    PageWidth = 800,
                    PageHeight = 600
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}