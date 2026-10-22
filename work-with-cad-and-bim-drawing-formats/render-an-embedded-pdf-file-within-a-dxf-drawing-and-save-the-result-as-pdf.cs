using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadPdfRenderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DXF file that contains an embedded PDF.
                string sourcePath = "input.dxf";

                // Path for the resulting PDF file.
                string outputPath = "output.pdf";

                // Verify source file exists.
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists.
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DXF drawing.
                using (Image dxfImage = Image.Load(sourcePath))
                {
                    // Create PDF save options.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the rendered drawing as PDF.
                    dxfImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("DXF drawing rendered and saved as PDF successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}