using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input CAD file path
        string inputPath = @"C:\Data\sample.dwg";

        // Output PDF/A file path
        string outputPath = @"C:\Output\exported_region.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (outputDir == null)
        {
            Console.WriteLine("Invalid output path.");
            return;
        }
        Directory.CreateDirectory(outputDir);

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Define rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 0.5f,
                    PageWidth = 210f,   // A4 width in millimeters
                    PageHeight = 297f,  // A4 height in millimeters
                    AutomaticLayoutsScaling = false,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 },
                    Layouts = new[] { "Layout1", "Layout2" } // replace with actual layout names
                };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the selected region with the chosen layers to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed successfully: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during export: {ex.Message}");
        }
    }
}