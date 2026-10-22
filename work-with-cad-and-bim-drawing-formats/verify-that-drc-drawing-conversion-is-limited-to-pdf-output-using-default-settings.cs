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
            // Define input CAD file path
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Define output PDF file path
            string outputPath = Path.Combine(outputDir, "output.pdf");

            // Load CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options if needed
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;
                rasterOptions.PageHeight = 1200;
                rasterOptions.Layouts = new string[] { "Model" };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"CAD file converted to PDF successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}