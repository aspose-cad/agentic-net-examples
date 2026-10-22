using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadRenderingExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input IGES file path
                    string inputPath = "input.iges";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Load the IGES file
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Configure rasterization options with high quality (anti‑aliasing)
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.PageWidth = 1600;   // Width of the rendered page in pixels
                        rasterOptions.PageHeight = 1600;  // Height of the rendered page in pixels
                        rasterOptions.UnitType = UnitType.Millimeter; // Optional: set measurement unit

                        // Set high quality for text, hatch and arc rendering
                        rasterOptions.Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High,
                            Arc = RasterizationQualityValue.High
                        };

                        // Create PDF save options and assign the rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the rendered image as PDF
                        cadImage.Save(outputPath, pdfOptions);
                    }

                    Console.WriteLine("Rendering completed. PDF saved to: " + outputPath);
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}