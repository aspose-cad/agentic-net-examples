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
            // Output PDF file path
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the PLT file
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options.
            // Note: CadRasterizationOptions does not expose resolution properties.
            // Low‑resolution rendering can be influenced by page size or by rendering to bitmap,
            // but direct DPI settings are not available in the current API.
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // Preserve original drawing size
                PageWidth = cadImage.Width,
                PageHeight = cadImage.Height
            };

            // Set PDF export options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rendered PDF
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}