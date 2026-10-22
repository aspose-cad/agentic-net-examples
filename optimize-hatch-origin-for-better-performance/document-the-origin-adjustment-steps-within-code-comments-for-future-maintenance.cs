using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadConversionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputPath = "sample.dwg";
            // Output directory and file path
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "sample.pdf");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the CAD image
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

                // Origin adjustment step:
                // Use the correct enum spelling UnitType.Centimenter (centimeter) to define measurement units.
                rasterOptions.UnitType = UnitType.Centimenter;

                // Set scaling and page size (A4 dimensions in millimeters)
                rasterOptions.LineScale = 1.0f;
                rasterOptions.PageWidth = 210.0f;
                rasterOptions.PageHeight = 297.0f;
                rasterOptions.AutomaticLayoutsScaling = true;

                // Define margins to offset the drawing origin
                Margins margins = new Margins();
                margins.Left = 10;   // offset from left edge
                margins.Top = 10;    // offset from top edge
                margins.Right = 10;
                margins.Bottom = 10;
                rasterOptions.Margins = margins;

                // Assign rasterization options to PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD file as PDF
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"CAD file successfully saved to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}