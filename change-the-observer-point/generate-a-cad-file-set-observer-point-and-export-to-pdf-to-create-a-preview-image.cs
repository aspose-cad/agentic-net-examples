using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;

namespace CadPdfPreview
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input CAD file path (replace with an existing DWG/DXF file)
            string inputFilePath = "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input CAD file not found: {inputFilePath}");
                Console.WriteLine("Please provide a valid DWG/DXF file and retry.");
                return;
            }

            // Create output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);

            // Load the CAD drawing
            using (Image image = Image.Load(inputFilePath))
            {
                // Cast to CadImage to access CAD-specific members if needed
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Configure rasterization options, including observer point for preview
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set page size (in points)
                    PageWidth = 800,
                    PageHeight = 600,
                    // Set observer point to control the view angle
                    ObserverPoint = new ObserverPoint(0, 0, 10)
                };

                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Define output PDF file path
                string outputFilePath = Path.Combine(outputDir, "preview.pdf");

                // Save the CAD drawing as PDF using the configured options
                cadImage.Save(outputFilePath, pdfOptions);

                Console.WriteLine($"PDF preview generated at: {outputFilePath}");
            }
        }
    }
}