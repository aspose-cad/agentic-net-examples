using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input PLT file and output PDF file paths
        string inputPath = @"C:\Input\sample.plt";
        string outputPath = @"C:\Output\sample.pdf";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (outputDir == null || !Directory.Exists(outputDir))
            {
                Console.Error.WriteLine($"Output directory does not exist: {outputDir}");
                return;
            }

            // Load the PLT file using Aspose.CAD.Image.Load
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options with a custom page size (e.g., 8.5 x 11 inches at 300 DPI)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Page size in pixels (width = inches * DPI, height = inches * DPI)
                    PageWidth = (int)(8.5f * 300),
                    PageHeight = (int)(11f * 300)
                };

                // Set up PDF export options and assign the rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the PLT as a PDF file
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Successfully converted PLT to PDF: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}