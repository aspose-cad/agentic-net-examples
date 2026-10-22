using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class RenderIgesToPdf
{
    static void Main()
    {
        string sourcePath = "sample.iges";
        string outputPath = "output.pdf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Load the IGES file into an Aspose.CAD Image object
            Image cadImage = Image.Load(sourcePath);

            // Configure PDF export options
            var pdfOptions = new PdfOptions();

            // NOTE: The current Aspose.CAD PdfOptions API does not expose
            // properties for setting a background color or enabling anti‑aliasing.
            // Therefore these settings cannot be applied.

            // Ensure the output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            // Save the CAD drawing as a PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine("IGES file has been rendered to PDF.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}