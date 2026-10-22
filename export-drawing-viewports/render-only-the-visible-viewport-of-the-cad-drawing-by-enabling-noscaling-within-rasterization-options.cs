using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Input CAD file path
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                string outputPath = "output.pdf";

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization to render only the visible viewport
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.NoScaling = true;

                    // Set PDF save options with the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}