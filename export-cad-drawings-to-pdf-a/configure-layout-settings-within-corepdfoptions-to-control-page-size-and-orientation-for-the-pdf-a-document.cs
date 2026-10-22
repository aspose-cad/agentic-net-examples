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
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "output.pdf");

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (e.g., layout selection, page size)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new string[] { "Model" },
                    PageWidth = 595,   // A4 width in points
                    PageHeight = 842   // A4 height in points
                };

                // Set PDF save options and attach rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}