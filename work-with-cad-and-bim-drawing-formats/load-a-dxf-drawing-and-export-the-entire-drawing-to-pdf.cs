using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source DXF file
        string inputFile = "input.dxf";

        // Verify input file exists
        if (!File.Exists(inputFile))
        {
            Console.Error.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Path for the resulting PDF file
        string outputFile = "output.pdf";

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DXF drawing
            using (Image dxfImage = Image.Load(inputFile))
            {
                // Configure PDF export options (default options are sufficient for full drawing export)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the entire drawing to PDF
                dxfImage.Save(outputFile, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}