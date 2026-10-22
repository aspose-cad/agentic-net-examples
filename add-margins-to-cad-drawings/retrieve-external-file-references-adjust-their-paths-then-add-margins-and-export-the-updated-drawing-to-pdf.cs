using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputDir = "Output";
        string outputPath = Path.Combine(outputDir, "Result.pdf");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image image = Image.Load(inputPath))
            {
                // Adjust external references if supported (placeholder)
                if (image is CadImage cadImage)
                {
                    // The current Aspose.CAD version does not expose external references directly.
                    // Implement custom handling here if required.
                }

                // Set up PDF export options with margins via rasterization options
                var pdfOptions = new PdfOptions();
                var rasterOptions = new CadRasterizationOptions
                {
                    // Define page size larger than the drawing to create margins (e.g., A4 size in mm)
                    PageWidth = 210f,   // A4 width in mm
                    PageHeight = 297f   // A4 height in mm
                    // If a centering option exists, it can be set here.
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save to PDF
                image.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
