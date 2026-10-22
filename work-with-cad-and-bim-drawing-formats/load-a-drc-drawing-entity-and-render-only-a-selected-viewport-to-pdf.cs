using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths
        string sourcePath = "input.drc";
        string outputPath = "output.pdf";

        // Validate source file existence
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DRC drawing
            Image cadImage = Image.Load(sourcePath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // Note: The Aspose.CAD API does not expose a Viewport property.
                // Rendering a specific viewport is not supported directly.
                // As a workaround, you can adjust PageWidth/PageHeight to approximate the desired area,
                // but the full drawing will be rendered.
                PageWidth = 800,   // example width
                PageHeight = 600   // example height
            };

            // Set up PDF export options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save to PDF
            cadImage.Save(outputPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}