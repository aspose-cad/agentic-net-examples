using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dwg";
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
                if (cadImage is Aspose.CAD.FileFormats.Cad.CadImage cadImg)
                {
                    cadImg.DefaultFont = "Arial";
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}
