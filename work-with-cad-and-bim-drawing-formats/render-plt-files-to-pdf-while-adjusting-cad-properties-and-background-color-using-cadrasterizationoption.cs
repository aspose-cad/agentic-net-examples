using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class PltToPdfRenderer
{
    static void Main(string[] args)
    {
        // Input PLT file path
        string inputPath = @"C:\Input\sample.plt";
        // Output PDF file path
        string outputPath = @"C:\Output\sample.pdf";

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the PLT file as a CadImage
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example page size; adjust as needed
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Note: BackgroundColor property uses System.Drawing.Color, which is prohibited in this sample.
                    // Therefore, background color adjustment is omitted.
                };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully rendered PLT to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}