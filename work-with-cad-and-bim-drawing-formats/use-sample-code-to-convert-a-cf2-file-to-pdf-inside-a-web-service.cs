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
            // Path to the source CF2 file
            string sourcePath = "input.cf2";
            // Path to the output PDF file
            string outputPath = "output.pdf";

            // Guard against missing source file
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

            // Load the CF2 file using Aspose.CAD.Image.Load
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing as a PDF document
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}