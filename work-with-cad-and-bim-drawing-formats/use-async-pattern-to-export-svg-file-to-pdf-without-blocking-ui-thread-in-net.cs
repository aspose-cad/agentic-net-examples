using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static async Task Main(string[] args)
    {
        // Input SVG file path
        string inputPath = "input.svg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Output PDF file path
        string outputPath = "output.pdf";

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        // Load the SVG image and export to PDF
        try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    PageWidth = 800,
                    PageHeight = 600,
                    AutomaticLayoutsScaling = true,
                    Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    }
                };

                // Create PDF export options and assign rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save to PDF asynchronously to avoid blocking UI thread
                try
                {
                    await Task.Run(() => image.Save(outputPath, pdfOptions));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving PDF: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine("SVG has been exported to PDF successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing SVG file: {ex.Message}");
        }
    }
}