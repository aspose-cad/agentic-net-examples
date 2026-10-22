using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input DXF file path
        string sourcePath = "input.dxf";

        // Output PDF file path
        string pdfPath = "output.pdf";

        // Output PNG image file path for visual verification
        string pngPath = "output.png";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Ensure output directories exist
            EnsureDirectory(pdfPath);
            EnsureDirectory(pngPath);

            // Load the DXF drawing
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Export to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);

                // Export to PNG image
                PngOptions pngOptions = new PngOptions();
                cadImage.Save(pngPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void EnsureDirectory(string filePath)
    {
        string? dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}