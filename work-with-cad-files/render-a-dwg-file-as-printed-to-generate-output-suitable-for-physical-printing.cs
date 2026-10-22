using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPrintRender
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DWG file path
            string inputPath = @"C:\CAD\sample.dwg";
            // Output PDF file path
            string outputPath = @"C:\CAD\output\sample_print.pdf";

            try
            {
                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Configure rasterization options for printing (A4 size at 300 DPI)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Page size in pixels (A4 @ 300 DPI)
                        PageWidth = 2480f,
                        PageHeight = 3508f,
                        // Use the model layout; adjust if a specific layout name is required
                        Layouts = new string[] { "Model" },
                        // Preserve object colors
                        DrawType = CadDrawTypeMode.UseObjectColor
                        // BackgroundColor omitted to avoid System.Drawing dependency
                    };

                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as a PDF suitable for physical printing
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully rendered and saved PDF to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during rendering:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}