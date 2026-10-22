using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "sample.dwg";

            // Verify that the source file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Destination PDF file path
            string outputPath = Path.Combine(outputDir, "sample.pdf");

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific options
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set page size (A4 in points)
                    PageWidth = 595,
                    PageHeight = 842,
                    // High quality rendering for text, hatch and arcs
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    },
                    // Render all layers and layouts (null means all)
                    Layers = null,
                    Layouts = null
                };

                // Set PDF save options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}