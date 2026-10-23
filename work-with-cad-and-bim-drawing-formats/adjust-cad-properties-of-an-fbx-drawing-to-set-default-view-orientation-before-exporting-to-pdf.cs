using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.fbx";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the FBX drawing
            using (Image image = Image.Load(inputPath))
            {
                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options if needed (e.g., page size)
                pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions
                {
                    // Example settings; adjust as required
                    PageWidth = 800,
                    PageHeight = 600
                };

                // NOTE: Aspose.CAD does not expose a property to set a default view orientation
                // for FBX (non‑native CAD) files. The export will use the current view as stored
                // in the file.

                // Export the drawing to PDF
                image.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF exported successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}