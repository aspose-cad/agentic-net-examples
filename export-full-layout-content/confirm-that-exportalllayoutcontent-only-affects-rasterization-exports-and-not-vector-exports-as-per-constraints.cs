using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputPdfPath = "output.pdf";
        string outputDxfPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string pdfDir = Path.GetDirectoryName(outputPdfPath);
        if (!string.IsNullOrEmpty(pdfDir) && !Directory.Exists(pdfDir))
        {
            Directory.CreateDirectory(pdfDir);
        }

        string dxfDir = Path.GetDirectoryName(outputDxfPath);
        if (!string.IsNullOrEmpty(dxfDir) && !Directory.Exists(dxfDir))
        {
            Directory.CreateDirectory(dxfDir);
        }

        try
        {
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Raster export to PDF with ExportAllLayoutContent enabled
            PdfOptions pdfOptions = new PdfOptions();
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
            rasterOptions.ExportAllLayoutContent = true;
            pdfOptions.VectorRasterizationOptions = rasterOptions;

            cadImage.Save(outputPdfPath, pdfOptions);
            Console.WriteLine($"PDF saved to {outputPdfPath} (raster export, ExportAllLayoutContent enabled).");

            // Vector export to DXF (ExportAllLayoutContent has no effect)
            DxfOptions dxfOptions = new DxfOptions();
            cadImage.Save(outputDxfPath, dxfOptions);
            Console.WriteLine($"DXF saved to {outputDxfPath} (vector export, ExportAllLayoutContent ignored).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}