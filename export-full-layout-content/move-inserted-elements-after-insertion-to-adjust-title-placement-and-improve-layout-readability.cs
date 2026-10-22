using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source CAD file
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options for vector output (PDF)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;   // Width of the output page in pixels
                    rasterOptions.PageHeight = 1200;  // Height of the output page in pixels
                    // Optional: specify layouts to render (e.g., "Model")
                    rasterOptions.Layouts = new string[] { "Model" };

                    // Set PDF save options and attach rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Path for the resulting PDF file
                    string outputPath = "output.pdf";

                    // Save the CAD drawing as PDF
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