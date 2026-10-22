using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "input.dxf";
        string outputPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
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
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter, // SDK's actual enum spelling
                    PageWidth = 800,                 // Width in points
                    PageHeight = 600,                // Height in points
                    LineScale = 0.5f                 // Scale for line widths
                    // FontsFolder property is not available in the current API version
                };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Conversion succeeded: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}