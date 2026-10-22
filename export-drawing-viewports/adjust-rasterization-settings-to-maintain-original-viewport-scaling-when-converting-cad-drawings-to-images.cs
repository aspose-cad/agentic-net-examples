using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string inputPath = "sample.dwg";

        // Directory where the output will be saved
        string outputDir = "output";

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
            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options to preserve original viewport scaling
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter; // Keep original units (spelling as per SDK)
                rasterOptions.LineScale = 1.0f;                 // No additional scaling
                rasterOptions.AutomaticLayoutsScaling = false; // Preserve layout size

                // Set margins (no extra margins)
                rasterOptions.Margins = new Margins();
                rasterOptions.Margins.Left = 0;
                rasterOptions.Margins.Right = 0;
                rasterOptions.Margins.Top = 0;
                rasterOptions.Margins.Bottom = 0;

                // Set up PDF export options and assign the rasterization settings
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Build the output file path
                string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");

                // Save the CAD drawing as a PDF using the configured options
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}