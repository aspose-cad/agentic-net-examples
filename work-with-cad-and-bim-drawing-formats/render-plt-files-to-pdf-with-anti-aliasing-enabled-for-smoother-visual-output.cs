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
            // Input PLT file path
            string inputPath = "sample.plt";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "sample.pdf");

            // Load the PLT file
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // Anti-aliasing is not available in the current Aspose.CAD API.
                PageWidth = 1600,
                PageHeight = 1200
            };

            // Set PDF save options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save as PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}