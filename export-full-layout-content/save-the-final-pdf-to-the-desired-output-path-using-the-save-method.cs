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
                string sourcePath = Path.Combine(dataDir, "sample.dwg");

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("Source CAD file not found: " + sourcePath);
                    return;
                }

                // Prepare output directory and file path
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "sample.pdf");

                // Load CAD image
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Optional: configure rasterization options for vector rendering
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1600;
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("PDF saved to: " + outputPath);
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}