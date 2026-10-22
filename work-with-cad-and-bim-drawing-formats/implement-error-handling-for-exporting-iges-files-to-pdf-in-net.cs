using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.ImageOptions; // For CadRenderResult, RenderResult

namespace CadIgesToPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.iges";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the IGES file with error ignoring enabled
                LoadOptions loadOptions = new LoadOptions
                {
                    IgnoreErrors = true
                };

                using (Image cadImage = Image.Load(inputPath, loadOptions))
                {
                    // Report any load errors
                    if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                    {
                        Console.Error.WriteLine("Load completed with errors:");
                        foreach (RenderResult loadError in loadOptions.Errors)
                        {
                            Console.Error.WriteLine($"Message: {loadError.Message}, Code: {loadError.RenderCode}");
                        }
                    }

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Configure rasterization options (required for vector export)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1200,
                        PageHeight = 1200,
                        Layouts = new[] { "Model" }
                    };

                    // Export callback to capture rendering problems
                    rasterOptions.RenderResult = (CadRenderResult renderResult) =>
                    {
                        if (!renderResult.IsRenderComplete)
                        {
                            Console.Error.WriteLine("Rendering incomplete. Failures:");
                        }

                        foreach (RenderResult failure in renderResult.Failures)
                        {
                            Console.Error.WriteLine($"Message: {failure.Message}, Code: {failure.RenderCode}");
                        }
                    };

                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the CAD image as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception during processing: {ex.Message}");
            }
        }
    }
}