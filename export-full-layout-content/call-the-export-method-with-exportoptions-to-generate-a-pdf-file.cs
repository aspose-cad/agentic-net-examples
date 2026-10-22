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
                // Define input and output directories
                string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");

                // Ensure the output directory exists
                Directory.CreateDirectory(outputDir);

                // Path to the source CAD file
                string sourcePath = Path.Combine(dataDir, "sample.dwg");

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("Source CAD file not found: " + sourcePath);
                    return;
                }

                // Load the CAD image
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Set up rasterization options (optional, can control page size, etc.)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1600;

                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Define the output PDF file path
                    string outputPath = Path.Combine(outputDir, "output.pdf");

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);

                    Console.WriteLine("PDF file saved to: " + outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}