using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                // Rasterization options (high‑quality settings can be adjusted here)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example of a high‑quality setting; adjust as needed
                    LineScale = 0.5f
                };

                // PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save as PDF (PDF/A compliance not available in this API version)
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"File saved successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}