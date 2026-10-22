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

        try
        {
            // Load the source CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to work with CAD‑specific features if needed
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options and specify the layers to export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,               // optional page width
                    PageHeight = 1200,              // optional page height
                    Layers = new[] { "Layer1", "Layer2" } // layers to include
                };

                // Choose PDF as the output format and assign the rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the drawing containing only the selected layers
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}