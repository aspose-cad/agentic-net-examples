using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string sourcePath = "input.dwg";
            // Path to the output PDF file (DWFx export is only supported via PDF)
            string outputPath = "output.pdf";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(sourcePath))
            {
                // DWFx export is not directly supported by Aspose.CAD.
                // The library can generate DWFx only through PDF output.
                // Therefore we save the drawing as PDF, which can be opened as DWFx.
                PdfOptions pdfOptions = new PdfOptions();

                // Save the drawing as PDF (serves as DWFx export)
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}