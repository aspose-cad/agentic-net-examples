using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputFilePath = "sample.dwg";

            // Output PDF file path
            string outputFilePath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                // Load CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
                {
                    // Configure rasterization options (optional, can control page size, layouts, etc.)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Example: set page size to A4
                        PageWidth = 595,
                        PageHeight = 842,
                        // Render all layouts
                        Layouts = new string[] { "Model" }
                    };

                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully exported to PDF: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during export: {ex.Message}");
            }
        }
    }
}