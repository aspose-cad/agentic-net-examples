using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWG file
                string sourcePath = "input.dwg";

                // Path for the resulting PDF
                string outputPath = "TopView.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Configure rasterization options for the specific layout
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,   // Set measurement unit
                    LineScale = 1.0f,                  // Preserve original line thickness
                    AutomaticLayoutsScaling = false, // Keep layout dimensions unchanged
                    Layouts = new[] { "TopView" },    // Export only the "TopView" layout
                    Margins = new Margins
                    {
                        Left = 0,
                        Right = 0,
                        Top = 0,
                        Bottom = 0
                    }
                };

                // Set PDF save options and attach rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Load the DWG file and save the selected layout to PDF
                using (Image cadImage = Image.Load(sourcePath))
                {
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}