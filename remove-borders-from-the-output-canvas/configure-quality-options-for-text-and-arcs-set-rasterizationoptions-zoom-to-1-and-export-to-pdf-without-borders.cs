using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "sample.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "result.pdf");

            using (Image cadImage = Image.Load(sourcePath))
            {
                if (!(cadImage is CadImage))
                {
                    Console.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Zoom = 1,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 },
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF exported successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}