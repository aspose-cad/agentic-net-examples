using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Configure rasterization options: select a non‑default layout and set custom canvas size
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Replace "Layout2" with the actual layout name you want to render
                    Layouts = new string[] { "Layout2" },

                    // Custom canvas size in pixels (example: 800x600)
                    PageWidth = 800,
                    PageHeight = 600
                };

                // Configure PDF save options and attach the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Load the CAD file and export to PDF
                using (Image cadImage = Image.Load(inputPath))
                {
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Export completed successfully: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}