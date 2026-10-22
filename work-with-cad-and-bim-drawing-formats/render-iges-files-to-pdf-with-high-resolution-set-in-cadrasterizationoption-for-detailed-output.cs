using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Determine input IGES file path
            string inputPath = args.Length > 0 ? args[0] : "sample.iges";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");

            // Configure high‑resolution rasterization options
            var rasterOptions = new CadRasterizationOptions
            {
                // Use a large page size to achieve high DPI (e.g., 300 DPI for A4)
                PageWidth = 2480,   // 8.27 inches * 300 DPI
                PageHeight = 3508,  // 11.69 inches * 300 DPI
                UnitType = UnitType.Centimenter,
                LineScale = 1.0f,
                AutomaticLayoutsScaling = false,
                Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
            };

            // Set PDF export options
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Load the IGES file and render to PDF
            using (Image image = Image.Load(inputPath))
            {
                image.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}