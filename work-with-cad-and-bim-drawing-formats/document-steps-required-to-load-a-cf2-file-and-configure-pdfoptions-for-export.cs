using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Input CF2 file path
        string inputPath = "sample.cf2";

        // Desired PDF output path
        string outputPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (Image cadImage = (Image)Image.Load(inputPath))
            {
                // Create PDF export options
                var pdfOptions = new PdfOptions();

                // Configure vector rasterization options using a concrete subclass
                pdfOptions.VectorRasterizationOptions = new SimpleVectorRasterizationOptions
                {
                    // Example settings (colors omitted to avoid System.Drawing usage)
                    ContentAsBitmap = false,
                    EmbedBackground = true,
                    BorderX = 0,
                    BorderY = 0
                };

                // Export to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF exported successfully to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export failed: {ex.Message}");
        }
    }
}

// Concrete implementation of the abstract VectorRasterizationOptions
class SimpleVectorRasterizationOptions : VectorRasterizationOptions
{
    // No additional members required if the base class has no abstract members
}