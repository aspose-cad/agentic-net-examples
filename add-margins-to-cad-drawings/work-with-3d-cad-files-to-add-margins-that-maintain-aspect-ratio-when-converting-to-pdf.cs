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
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                // Original drawing dimensions
                int originalWidth = cadImage.Width;
                int originalHeight = cadImage.Height;

                // Add 10% margin while preserving aspect ratio
                const double marginFactor = 0.10;
                int pageWidth = (int)(originalWidth * (1 + marginFactor));
                int pageHeight = (int)(originalHeight * (1 + marginFactor));

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = pageWidth,
                    PageHeight = pageHeight
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
