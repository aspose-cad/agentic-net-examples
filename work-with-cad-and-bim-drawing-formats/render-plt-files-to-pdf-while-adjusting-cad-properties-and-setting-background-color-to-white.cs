using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPltToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input PLT file path
            string inputPath = @"input\sample.plt";
            // Output PDF file path
            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
            string outputPath = Path.Combine(outputDirectory, "sample.pdf");

            try
            {
                // Verify source file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Source file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the PLT file as a generic Image (non‑native CAD format)
                using (Image image = Image.Load(inputPath))
                {
                    // Configure PDF export options with white background
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        BackgroundColor = Aspose.CAD.Color.White
                    };

                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image as PDF
                    image.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF successfully created at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}