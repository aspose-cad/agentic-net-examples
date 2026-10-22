using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Path for the resulting PDF file
            string outputPath = "output.pdf";

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example of setting a higher line scale for better detail
                    LineScale = 1.0f
                };

                // Assign the rasterization options to the PDF options
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the drawing as a PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}