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
                // Path to the source CAD file
                string inputPath = "sample.dwg";

        
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
                    // Cast to CadImage to access CAD-specific members if needed
                    CadImage cad = (CadImage)cadImage;

                    // Configure rasterization options (optional, e.g., page size and layout)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1200;
                    rasterOptions.Layouts = new string[] { "Model" };

                    // Set up PDF export options using the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing as a PDF file
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