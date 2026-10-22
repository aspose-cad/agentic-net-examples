using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input IGES file path
        string sourcePath = "input.iges";
        // Desired PDF output path
        string outputPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Input file not found: {sourcePath}");
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
            // Load the IGES file using Aspose.CAD
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to work with CAD‑specific features if needed
                Image cadImage = image ;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Configure PDF save options (vector output preserves original line colors)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}