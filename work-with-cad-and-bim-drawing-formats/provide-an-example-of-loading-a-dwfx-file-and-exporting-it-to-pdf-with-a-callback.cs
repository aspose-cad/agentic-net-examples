using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DWFx file path
            string inputPath = "sample.dwf";
            // Output PDF file path
            string outputPath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Configure load options
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the CAD document
                Image cadImage = Image.Load(inputPath, loadOptions);

                // Configure rasterization options with a render callback
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Inch,
                    PageWidth = 8.5f,
                    PageHeight = 11f
                };

                // Callback invoked after rendering each layout
                rasterOptions.RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.WriteLine("Render incomplete.");
                        if (result.Failures != null)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.WriteLine($"  Failure: {failure.Message}");
                            }
                        }
                    }
                };

                // Set up PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD document as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading CAD file: " + ex.Message);
                // Inspect load errors if any
                foreach (var error in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {error.Message}");
                }
            }
        }
    }
}