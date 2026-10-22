using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
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
                    // Set up PDF export options.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Configure rasterization options if needed (e.g., page size, layout).
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600
                    };
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the CAD file as PDF.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully exported to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
