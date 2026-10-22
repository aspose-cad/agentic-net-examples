using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.plt";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the PLT file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)image;

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set page size (A4 in points, 1 point = 1/72 inch)
                    PageWidth = 595f,   // 8.27 inches * 72
                    PageHeight = 842f,  // 11.69 inches * 72
                    // Export only the "Model" layout (adjust as needed)
                    Layouts = new[] { "Model" }
                };

                // Create PDF export options and assign the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the PLT drawing as a PDF file
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}