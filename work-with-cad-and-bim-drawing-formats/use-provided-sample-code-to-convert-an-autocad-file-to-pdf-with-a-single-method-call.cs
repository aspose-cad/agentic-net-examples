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
            // Path to the source AutoCAD file
            string sourceFile = "input.dwg";
            // Path for the output PDF file
            string outputFile = "output.pdf";

            // Verify source file exists
            if (!File.Exists(sourceFile))
            {
                Console.Error.WriteLine($"Source file not found: {sourceFile}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(sourceFile))
            {
                // Create PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD file as PDF
                cadImage.Save(outputFile, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}