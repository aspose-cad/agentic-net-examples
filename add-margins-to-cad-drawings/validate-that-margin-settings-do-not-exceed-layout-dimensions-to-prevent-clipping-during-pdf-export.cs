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

            // Desired margins (in pixels)
            const int marginTop = 20;
            const int marginBottom = 20;
            const int marginLeft = 20;
            const int marginRight = 20;

            // Base page size
            const int basePageWidth = 1600;
            const int basePageHeight = 1200;

            // Validate margins against page dimensions
            if (marginLeft + marginRight >= basePageWidth || marginTop + marginBottom >= basePageHeight)
            {
                Console.Error.WriteLine("Margins are too large for the specified page size. Adjusting to fit.");
            }

            // Calculate drawable area after applying margins
            int drawableWidth = Math.Max(0, basePageWidth - (marginLeft + marginRight));
            int drawableHeight = Math.Max(0, basePageHeight - (marginTop + marginBottom));

            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for the CAD drawing
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = drawableWidth,
                    PageHeight = drawableHeight,
                    Layouts = new[] { "Model" }
                };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as a PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
