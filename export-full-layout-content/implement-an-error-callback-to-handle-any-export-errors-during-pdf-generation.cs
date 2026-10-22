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
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        var errorCallback = new ExportErrorCallback();

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific features
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options (size, etc.)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the drawing to PDF with error handling
                try
                {
                    cad.Save(outputPath, pdfOptions);
                }
                catch (Exception ex)
                {
                    errorCallback.OnError(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }

    // Simple error callback implementation (no external interface)
    private class ExportErrorCallback
    {
        public void OnError(string message)
        {
            Console.WriteLine("Export error: " + message);
        }
    }
}