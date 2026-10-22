using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;

namespace CadObserverPointExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file
            string sourceCadPath = "input.dwg";

            // Path for the resulting PDF file
            string outputPdfPath = "output.pdf";

            // Verify source file exists
            if (!File.Exists(sourceCadPath))
            {
                Console.Error.WriteLine($"Source CAD file not found: {sourceCadPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the CAD image
                Image cadImage = Image.Load(sourceCadPath);

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set the observer point using rotation angles (in degrees)
                    // Example: rotate 30 degrees around X axis, no rotation around Y and Z
                    ObserverPoint = new ObserverPoint(30f, 0f, 0f),

                    // Optional: define page size for the PDF output
                    PageWidth = 800,   // width in pixels
                    PageHeight = 600   // height in pixels
                };

                // Create PDF export options and assign the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF using the configured observer point
                cadImage.Save(outputPdfPath, pdfOptions);

                Console.WriteLine("PDF exported successfully to: " + outputPdfPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}