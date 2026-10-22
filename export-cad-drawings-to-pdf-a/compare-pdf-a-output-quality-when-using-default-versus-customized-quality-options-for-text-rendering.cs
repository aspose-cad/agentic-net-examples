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
            // Input CAD file path (replace with an actual file when running the sample)
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // ---------- Save with default PDF/A options ----------
                string defaultPdfPath = Path.Combine(outputDir, "DefaultPdfA.pdf");
                PdfOptions defaultPdfOptions = new PdfOptions();
                cadImage.Save(defaultPdfPath, defaultPdfOptions);
                Console.WriteLine($"Saved default PDF/A to: {defaultPdfPath}");

                // ---------- Save with customized rasterization options ----------
                string customPdfPath = Path.Combine(outputDir, "CustomPdfA.pdf");
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Increase page size to improve raster quality
                    PageWidth = cadImage.Width * 2,
                    PageHeight = cadImage.Height * 2,
                    // Increase line scale for sharper vector rendering
                    LineScale = 2.0f,
                    // Enable automatic scaling of layouts
                    AutomaticLayoutsScaling = true
                };

                PdfOptions customPdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(customPdfPath, customPdfOptions);
                Console.WriteLine($"Saved custom PDF/A to: {customPdfPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}