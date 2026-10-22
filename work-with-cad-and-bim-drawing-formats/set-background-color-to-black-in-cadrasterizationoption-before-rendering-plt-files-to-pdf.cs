using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source PLT file
        string sourcePath = "input.plt";

        // Path where the PDF will be saved
        string outputPath = "output.pdf";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
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
            // Load the PLT file
            using (Image image = Image.Load(sourcePath))
            {
                // Configure rasterization options.
                // Note: Setting a background color requires System.Drawing.Color, which is prohibited in this console-only sample.
                var rasterOptions = new CadRasterizationOptions();

                // Set up PDF save options and attach the rasterization options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered PDF
                image.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}