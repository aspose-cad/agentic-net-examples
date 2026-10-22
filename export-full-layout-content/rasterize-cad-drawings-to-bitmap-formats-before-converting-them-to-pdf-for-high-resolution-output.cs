using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the source CAD file (DWG, DXF, DGN, etc.)
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file '{inputPath}' not found. Exiting.");
                return;
            }

            // Create output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);

            // Load CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage. Exiting.");
                    return;
                }

                // Configure rasterization options for high‑resolution bitmap
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Approximate A4 size at 300 DPI (pixels)
                    PageWidth = 2480,
                    PageHeight = 3508
                };

                // Save rasterized bitmap (PNG)
                string pngPath = Path.Combine(outputDir, "rasterized.png");
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cad.Save(pngPath, pngOptions);
                Console.WriteLine($"Rasterized PNG saved to: {pngPath}");

                // Load the rasterized PNG
                using (Image rasterImage = Image.Load(pngPath))
                {
                    // Save the PNG as PDF
                    string pdfPath = Path.Combine(outputDir, "output.pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    rasterImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"PDF created from rasterized image: {pdfPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}