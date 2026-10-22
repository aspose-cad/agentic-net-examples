using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace ExportSelectedLayersWithRescaling
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (replace with actual path)
                string inputPath = "input.dwg";

                // Output PDF file path
                string outputPath = "output.pdf";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Specify the layers you want to export
                    string[] selectedLayers = new string[] { "Layer1", "Layer2" };

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Use the correct enum spelling from the SDK
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        AutomaticLayoutsScaling = true,
                        PageWidth = 21.0f,   // A4 width in centimeters
                        PageHeight = 29.7f, // A4 height in centimeters
                        Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 },
                        Layouts = null,      // Export all layouts
                        Layers = selectedLayers
                    };

                    // Set PDF save options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD file as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"CAD file has been exported to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}