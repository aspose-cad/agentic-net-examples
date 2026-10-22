using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadIgesToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input IGES file path (replace with actual path)
            string inputPath = "sample.iges";
            // Output PDF file path
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the IGES file
                Image cadImage = Image.Load(inputPath);

                // Configure rasterization options.
                // Note: CadRasterizationOptions does not have a Resolution property.
                // Low‑resolution rendering can be approximated by rendering as bitmap.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    ContentAsBitmap = true
                };

                // Set up PDF export options and assign rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"Successfully rendered IGES to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}