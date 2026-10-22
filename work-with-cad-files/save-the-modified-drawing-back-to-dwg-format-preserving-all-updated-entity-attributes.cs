using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the source CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing (DWG, DXF, etc.) using Aspose.CAD.Image.Load
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for vector-to-raster conversion if needed
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example: set output page size (A4 in points)
                    PageWidth = 842,
                    PageHeight = 595,
                    // Additional options such as background color can be set here
                };

                // Set up PDF export options and attach rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing to PDF format
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD file successfully saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}