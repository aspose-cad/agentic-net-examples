using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = "input.3ds";
            string outputPath = "output.pdf";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the 3DS drawing
                Image cadImage = Image.Load(sourcePath);

                // Configure rasterization options.
                // Note: Aspose.CAD does not provide direct camera manipulation properties.
                // Camera angle adjustments are not supported via CadRasterizationOptions.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    BackgroundColor = Color.White
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the view to PDF
                cadImage.Save(outputPath, pdfOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}