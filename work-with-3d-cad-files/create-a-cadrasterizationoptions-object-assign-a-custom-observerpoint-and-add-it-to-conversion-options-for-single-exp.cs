using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    ObserverPoint = new ObserverPoint(30.0f, 45.0f, 0.0f),
                    UnitType = UnitType.Millimeter,
                    PageWidth = 800f,
                    PageHeight = 600f,
                    LineScale = 1.0f
                };

                // Create PDF export options and attach the rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing to PDF using the configured options
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}