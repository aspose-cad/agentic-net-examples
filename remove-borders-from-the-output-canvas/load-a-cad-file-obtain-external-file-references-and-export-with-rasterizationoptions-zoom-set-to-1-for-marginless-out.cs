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
            string sourcePath = "input.dwg";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD image
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Loaded file is not a supported CAD format.");
                    return;
                }

                // External file references are not exposed in the current SDK version.
                // Placeholder for handling external references if needed.
                Console.WriteLine("External file references enumeration is not supported in this SDK version.");

                // Prepare rasterization options with Zoom = 1 for marginless output
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Zoom = 1,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Choose PDF as the export format
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Ensure output directory exists
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);

                // Save the exported PDF
                string outputPath = Path.Combine(outputDir, "output.pdf");
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}