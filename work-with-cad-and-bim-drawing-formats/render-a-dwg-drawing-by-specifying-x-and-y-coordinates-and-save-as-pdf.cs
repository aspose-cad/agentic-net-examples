using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadRenderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DWG file path (replace with actual path)
            string inputPath = "input.dwg";
            // Output PDF file path (replace with desired path)
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DWG drawing
                Image cadImage = Image.Load(inputPath);

                // Configure rasterization options.
                // Note: CadRasterizationOptions does not provide PositionX/PositionY properties,
                // so specifying exact X and Y coordinates for rendering is not supported by the SDK.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Optional: set page size if required
                    PageWidth = 800,
                    PageHeight = 600
                };

                // Set up PDF export options using the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered drawing as PDF
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"DWG rendering completed successfully. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}