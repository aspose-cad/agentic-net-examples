using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace ExportFullLayoutContent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output PDF file path
                string outputPath = "output.pdf";

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Retrieve all layout names from the CAD file
                    string[] layoutNames = cadImage.Layouts.Keys.Cast<string>().ToArray();

                    // Configure rasterization options for vector output
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Export all layouts
                        Layouts = layoutNames,
                        // Optional: set page size, etc.
                        PageWidth = 1200,
                        PageHeight = 800
                    };

                    // Configure PDF save options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD file as PDF with full layout content
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}