using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
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
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options for PDF export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Additional options such as Layouts or Layers can be set here if needed
                    };

                    // Configure PDF save options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"File successfully saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}