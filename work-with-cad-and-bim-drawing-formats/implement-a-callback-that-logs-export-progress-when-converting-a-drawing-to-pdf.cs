using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadToPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD drawing
                string sourcePath = "input.dwg";
                // Path for the resulting PDF file
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Configure load options (ignore errors to continue processing)
                var loadOptions = new LoadOptions();
                loadOptions.IgnoreErrors = true;

                // Load the CAD image
                using (Image cadImage = Image.Load(sourcePath, loadOptions))
                {
                    // Prepare rasterization options for PDF export
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,   // example width in points
                        PageHeight = 600,  // example height in points
                        UnitType = UnitType.Point,
                        AutomaticLayoutsScaling = true
                    };

                    // Attach a callback to log rendering progress
                    rasterOptions.RenderResult = renderResult =>
                    {
                        Console.WriteLine($"Render Complete: {renderResult.IsRenderComplete}");
                        if (renderResult.Failures != null && renderResult.Failures.Count > 0)
                        {
                            foreach (var failure in renderResult.Failures)
                            {
                                Console.WriteLine($"Render Failure: {failure.Message} (Code: {failure.RenderCode})");
                            }
                        }
                    };

                    // Set up PDF export options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("Export completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}