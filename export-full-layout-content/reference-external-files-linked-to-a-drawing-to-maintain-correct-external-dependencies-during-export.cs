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
            // Path to the source CAD file (DWG/DXF) that may contain external references
            string sourcePath = "input.dwg";

            // Verify that the source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Path to the output PDF file
            string outputPath = "output.pdf";

            // Load the CAD drawing
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD‑specific members if needed
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options; external references are handled automatically
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,   // optional page width
                    PageHeight = 1200   // optional page height
                };

                // Create PDF save options and assign the rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing to PDF, preserving external file dependencies
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}