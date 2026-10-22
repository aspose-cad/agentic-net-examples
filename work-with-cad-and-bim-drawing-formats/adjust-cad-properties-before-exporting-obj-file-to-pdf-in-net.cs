using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.obj";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                PdfOptions pdfOptions = new PdfOptions();

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1024,
                    PageHeight = 768,
                    Layouts = new string[] { "Model" }
                    // BackgroundColor omitted to avoid System.Drawing usage
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}