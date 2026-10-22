using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.stl";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the STL file
            using (Image stlImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members if needed
                CadImage cadImage = (CadImage)stlImage;

                // Configure PDF export options with rasterization settings
                PdfOptions pdfOptions = new PdfOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,   // Set desired page width
                    PageHeight = 600   // Set desired page height
                    // BackgroundColor omitted to avoid System.Drawing usage
                };

                // Assign rasterization options to PDF options
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Export the STL to PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}