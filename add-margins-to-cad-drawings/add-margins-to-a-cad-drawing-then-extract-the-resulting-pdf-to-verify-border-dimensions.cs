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
            // Input CAD file path (replace with an actual file when available)
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file
            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);
            string outputPdfPath = Path.Combine(outputDir, "sample_with_margins.pdf");

            // Define margin size (in points; adjust units via PageUnits if needed)
            float margin = 10f;

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Original dimensions of the drawing (in points)
                float originalWidth = cadImage.Width;
                float originalHeight = cadImage.Height;

                // Configure rasterization options with added margins
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = originalWidth + 2 * margin,
                    PageHeight = originalHeight + 2 * margin,
                    // Optional: set page units if needed, e.g., PageUnits = PageUnits.Points
                };

                // Set PDF save options using the rasterization settings
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as PDF with margins
                cadImage.Save(outputPdfPath, pdfOptions);
            }

            // Simple verification that the PDF was created
            if (File.Exists(outputPdfPath))
            {
                FileInfo pdfInfo = new FileInfo(outputPdfPath);
                Console.WriteLine($"PDF saved successfully: {outputPdfPath}");
                Console.WriteLine($"File size: {pdfInfo.Length} bytes");
                Console.WriteLine($"Margins applied: {margin} points on each side");
            }
            else
            {
                Console.Error.WriteLine("Failed to create the PDF file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
