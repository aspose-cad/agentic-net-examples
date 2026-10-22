using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadAntiAliasingExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input FBX file path
            string inputFile = "input.fbx";
            // Output PDF file path
            string outputFile = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputFile))
                {
                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Set rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1600
                        // Note: Aspose.CAD does not provide a property to enable anti‑aliasing directly.
                        // Anti‑aliasing cannot be configured via GraphicsOptions in the current SDK version.
                    };

                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing as PDF with the specified options
                    cadImage.Save(outputFile, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}