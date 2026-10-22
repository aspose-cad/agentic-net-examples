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
            // Path to the source DWF file (replace with actual file location)
            string sourcePath = "input.dwf";
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

            // Load the DWF drawing using the generic CAD image loader
            using (Image dwfImage = Image.Load(sourcePath))
            {
                // Create PDF export options with default settings.
                // Default conversion rasterizes the drawing to PDF pages,
                // preserving the original drawing size, layers and vector information.
                PdfOptions pdfOptions = new PdfOptions();

                // Save the loaded DWF drawing as a PDF file using the default conversion process.
                dwfImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}