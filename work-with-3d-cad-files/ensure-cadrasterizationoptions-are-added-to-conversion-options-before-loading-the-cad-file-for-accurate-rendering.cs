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
                // Path to the source CAD file
                string sourcePath = "input.dwg";

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Path for the exported PDF file
                string outputPath = "output.pdf";

                // Configure rasterization options (e.g., select specific layout)
                CadRasterizationOptions cadRasterizationOptions = new CadRasterizationOptions
                {
                    Layouts = new string[] { "Layout1" }
                    // Additional rasterization settings can be configured here, e.g.,
                    // Layers = new string[] { "LayerA", "LayerB" },
                    // PageWidth = 800,
                    // PageHeight = 600
                };

                // Configure PDF export options and attach the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = cadRasterizationOptions
                };

                // Load the CAD file and export to PDF
                using (Image cadImage = Image.Load(sourcePath))
                {
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("CAD file has been successfully exported to PDF.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}