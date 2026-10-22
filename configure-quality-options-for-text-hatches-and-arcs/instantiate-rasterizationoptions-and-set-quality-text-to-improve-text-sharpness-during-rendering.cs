using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations.
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the source CAD file exists.
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options for vector-to-PDF conversion.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Example page size; adjust as needed.
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Render the "Model" layout; modify for other layout names.
                        Layouts = new string[] { "Model" }
                    };

                    // Set up PDF export options.
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PDF.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion succeeded. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}