using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source CAD file (DWG, DXF, etc.)
                string inputFile = "sample.dwg";

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputFile))
                {
                    // ---------- Export to PDF ----------
                    PdfOptions pdfOptions = new PdfOptions();
                    string pdfOutput = "output.pdf";
            
                    string outputDir = Path.GetDirectoryName(pdfOutput);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
        cadImage.Save(pdfOutput, pdfOptions);

                    // ---------- Export to PNG ----------
                    // Create rasterization options to control output size and quality
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;   // Width in pixels
                    rasterOptions.PageHeight = 1200;  // Height in pixels

                    // Assign rasterization options to PNG options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    string pngOutput = "output.png";
                    cadImage.Save(pngOutput, pngOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}