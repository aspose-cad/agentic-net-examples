using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "Resources/sample.dwg";
        string outputDir = "Output";

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

            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Export to PDF (vector)
            PdfOptions pdfOptions = new PdfOptions();
            CadRasterizationOptions pdfRasterOptions = new CadRasterizationOptions
            {
                // Example: set page size to A4 (in points)
                PageWidth = 842,   // width in points
                PageHeight = 595   // height in points
                // Note: CadRasterizationOptions does not expose a RenderMode property in the current API,
                // so specifying a "Printed" render mode is not supported.
            };
            pdfOptions.VectorRasterizationOptions = pdfRasterOptions;

            string pdfPath = Path.Combine(outputDir, "output.pdf");
            cadImage.Save(pdfPath, pdfOptions);
            Console.WriteLine($"PDF saved to: {pdfPath}");

            // Export to PNG (raster)
            PngOptions pngOptions = new PngOptions();
            CadRasterizationOptions pngRasterOptions = new CadRasterizationOptions
            {
                // Define raster size via page dimensions (the SDK does not provide direct Width/Height properties)
                PageWidth = 1600,   // desired width in points
                PageHeight = 1200   // desired height in points
                // Note: CadRasterizationOptions does not expose a RenderMode property in the current API,
                // so specifying a "Printed" render mode is not supported.
            };
            pngOptions.VectorRasterizationOptions = pngRasterOptions;

            string pngPath = Path.Combine(outputDir, "output.png");
            cadImage.Save(pngPath, pngOptions);
            Console.WriteLine($"PNG saved to: {pngPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}