using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.pdf");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD file
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Create PDF export options
            PdfOptions pdfOptions = new PdfOptions();

            // Configure vector rasterization options for PDF (CF2 conversion)
            // Use the concrete CadRasterizationOptions implementation
            CadRasterizationOptions vectorOptions = new CadRasterizationOptions
            {
                // Example: set rasterization resolution (DPI)
                // Resolution = 300,

                // Example: set page size if needed (default is A4)
                // PageWidth = 842,   // A4 width in points
                // PageHeight = 595   // A4 height in points
            };

            pdfOptions.VectorRasterizationOptions = vectorOptions;

            // Save the CAD drawing as PDF
            cadImage.Save(outputPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}