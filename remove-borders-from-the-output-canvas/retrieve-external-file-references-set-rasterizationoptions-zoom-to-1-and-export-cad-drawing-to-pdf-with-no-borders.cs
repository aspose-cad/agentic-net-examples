using Aspose.CAD.FileFormats.Cad;
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
            string sourcePath = "sample.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file '{sourcePath}' not found.");
                return;
            }

            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);
            string pdfPath = Path.Combine(outputDir, "output.pdf");

            // Load the CAD file
            using (Image image = Image.Load(sourcePath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Set rasterization options: Zoom = 1, page size matches image dimensions, no margins
                var rasterizationOptions = new CadRasterizationOptions
                {
                    Zoom = 1,
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Configure PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

                // Export to PDF
                cadImage.Save(pdfPath, pdfOptions);
            }

            Console.WriteLine($"PDF saved to: {pdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}