using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class RenderIgesToPdf
{
    static void Main()
    {
        // Path to the source IGES file
        string sourcePath = "input.iges";

        // Path for the resulting PDF file
        string outputPath = "output.pdf";

        // Verify source file exists
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
            // Load the IGES file
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Adjust CAD properties if needed (example: set default font)
                if (cadImage is CadImage cad)
                {
                    cad.DefaultFont = "Arial";
                }

                // Configure PDF export options with a white background (default is white)
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        // Use the original image dimensions
                        PageWidth = (float)cadImage.Width,
                        PageHeight = (float)cadImage.Height
                        // BackgroundColor defaults to white; no UI namespace used
                    }
                };

                // Save the rendered PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("IGES file has been rendered to PDF with a white background.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}