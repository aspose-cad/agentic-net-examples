using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPdfExport
{
    class Program
    {
        static void Main(string[] args)
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
                using (Image cadImageBase = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)cadImageBase;

                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Adjust line scale to improve rendering quality for arcs
                        LineScale = 0.5f,
                        PageWidth = 1200,
                        PageHeight = 800
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
}