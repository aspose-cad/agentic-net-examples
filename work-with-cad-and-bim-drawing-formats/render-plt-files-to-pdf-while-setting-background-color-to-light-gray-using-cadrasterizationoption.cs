using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class PltToPdfRenderer
{
    static void Main()
    {
        string sourcePath = "input.plt";
        string outputPath = "output.pdf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            using (Image cadImage = Image.Load(sourcePath))
            {
                // CadRasterizationOptions can be configured, but PdfOptions does not expose a RasterizationOptions property.
                // Therefore background color cannot be set directly for PDF output using the current SDK.
                var pdfOptions = new PdfOptions();

                // Save the rendered PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}