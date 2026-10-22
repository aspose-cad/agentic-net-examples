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
                // Define input CAD file path
                string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                string inputPath = Path.Combine(dataDir, "sample.dwg");

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory and file path
                string outDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outDir);
                string outputPath = Path.Combine(outDir, "sample.pdf");

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options with Zoom = 1
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.Zoom = 1;

                    // Set up PDF export options using the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Export the CAD drawing to PDF while preserving text and hatch quality
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF saved to {outputPath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}