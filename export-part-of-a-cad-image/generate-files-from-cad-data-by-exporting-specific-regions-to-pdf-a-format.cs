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
            // Input CAD file path (replace with an actual file when running the sample)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);

            // Output PDF file path
            string outputPath = Path.Combine(outputDir, "ExportedRegion.pdf");

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PDF export options
                var pdfOptions = new PdfOptions
                {
                    // Define rasterization options for the region to export
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        // Example page size; adjust as needed for the desired region
                        PageWidth = 800,
                        PageHeight = 600
                    }
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