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
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PDF export options.
                PdfOptions pdfOptions = new PdfOptions();

                // CadRasterizationOptions control layout, layers, and raster settings.
                // Note: Quality settings (e.g., compression quality) are not applicable to vector formats like PDF.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    // Resolution is a raster concept; it does not affect vector PDF output.
                    // No Quality property exists for PdfOptions, confirming that quality is ignored.
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD image as PDF. Quality settings are inherently ignored.
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD file successfully converted to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}