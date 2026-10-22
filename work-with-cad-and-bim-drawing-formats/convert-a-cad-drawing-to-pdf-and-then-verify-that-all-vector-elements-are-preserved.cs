using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
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
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Count vector entities before conversion
                int entityCount = cadImage.Entities.Count();

                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Optional: control page size or other rasterization settings
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save as PDF
                cadImage.Save(outputPath, pdfOptions);

                // Verify that the PDF file was created and is not empty
                if (File.Exists(outputPath) && new FileInfo(outputPath).Length > 0)
                {
                    Console.WriteLine($"Conversion succeeded. Vector entities preserved: {entityCount}");
                }
                else
                {
                    Console.Error.WriteLine("Conversion failed: PDF file was not created or is empty.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}