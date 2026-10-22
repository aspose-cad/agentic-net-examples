using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string inputFilePath = "input.dwg";

        // Verify input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        // Path to the output file
        string outputFilePath = "output.pdf";

        // Ensure output directory exists
        try
        {
            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to prepare output directory: {ex.Message}");
            return;
        }

        // Load the CAD drawing and export to PDF
        try
        {
            using (Image cadImage = Image.Load(inputFilePath))
            {
                // Cast to CadImage to access CAD-specific members if needed
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options (page size, etc.)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,   // Width in pixels
                    PageHeight = 1200   // Height in pixels
                    // BackgroundColor omitted to avoid System.Drawing dependency
                };

                // Choose PDF as the output format and assign rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to PDF
                cadImage.Save(outputFilePath, pdfOptions);
                Console.WriteLine($"Export successful: {outputFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}
