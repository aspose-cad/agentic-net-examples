using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportAllLayoutContent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string sourcePath = "input.dwg";
                string outputPath = "output.pdf";

                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        ExportAllLayoutContent = true
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
                return;
            }
        }
    }
}