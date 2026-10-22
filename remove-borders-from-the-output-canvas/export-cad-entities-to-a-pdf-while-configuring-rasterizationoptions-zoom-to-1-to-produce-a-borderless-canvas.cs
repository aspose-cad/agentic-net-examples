using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG, DXF, DGN, etc.)
                string sourcePath = "input.dwg";

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Path for the resulting PDF file
                string outputPath = "output.pdf";

                // Load the CAD file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Set up rasterization options with Zoom = 1 for a borderless canvas
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Zoom = 1
                    };

                    // Assign the rasterization options to the PDF options
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the CAD drawing as a PDF file
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF successfully created at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}