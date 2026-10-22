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

        // Output directory and PDF file path
        string outputDir = @"C:\Data\Output";
        string outputPath = Path.Combine(outputDir, "sample_with_title.pdf");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(outputDir);

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                // Use the correct unit type enum (Centimenter)
                rasterOptions.UnitType = UnitType.Centimenter;
                // Set page size (A4 in centimenters: 2100 x 2970)
                rasterOptions.PageWidth = 2100f;
                rasterOptions.PageHeight = 2970f;
                // Add top margin to move drawing down and make space for a title
                rasterOptions.Margins = new Margins
                {
                    Top = 200,    // space for title (in centimenters)
                    Bottom = 0,
                    Left = 0,
                    Right = 0
                };
                // Enable automatic layout scaling
                rasterOptions.AutomaticLayoutsScaling = true;

                // Create PDF export options and assign rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the adjusted layout as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved successfully to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}