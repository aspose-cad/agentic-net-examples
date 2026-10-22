using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputPath = "output.pdf";

        // Validate input file existence
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
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options to improve arc smoothness
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 2000,          // larger width for higher pixel density
                    PageHeight = 2000,         // larger height for higher pixel density
                    LineScale = 2.0f,          // increase line scale for better curve fidelity
                    UnitType = UnitType.Millimeter
                };

                // Set PDF export options with the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}