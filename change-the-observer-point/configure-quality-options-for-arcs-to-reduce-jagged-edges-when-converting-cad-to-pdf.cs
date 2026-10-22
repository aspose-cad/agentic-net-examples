using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadToPdfWithHighArcQuality
{
    static void Main()
    {
        const string inputPath = "Input.dwg";
        const string outputPath = "Output.pdf";

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
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Quality = new RasterizationQuality
                    {
                        Arc = RasterizationQualityValue.High,
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High
                    }
                };

                var pdfOptions = new PdfOptions
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