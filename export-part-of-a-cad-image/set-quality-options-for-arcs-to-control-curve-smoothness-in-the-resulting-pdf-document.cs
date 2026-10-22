using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Determine input CAD file path (first argument or default placeholder)
                string inputPath = args.Length > 0 ? args[0] : "sample.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine("Input CAD file not found: " + inputPath);
                    return;
                }

                // Prepare output directory
                string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Define output PDF file path
                string outputPath = Path.Combine(outputDirectory, "result.pdf");

                // Configure rasterization options with high arc quality
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.Quality = new RasterizationQuality
                {
                    Text = RasterizationQualityValue.High,
                    Hatch = RasterizationQualityValue.High,
                    Arc = RasterizationQualityValue.High
                };
                // Optional: set page size (defaults will be used if not set)
                // rasterOptions.PageWidth = 800;
                // rasterOptions.PageHeight = 600;

                // Configure PDF save options
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Load the CAD image, apply options, and save as PDF
                using (Image cadImage = Image.Load(inputPath))
                {
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("PDF file saved to: " + outputPath);
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}