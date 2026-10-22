using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwf";
        string outputPath = "output.pdf";

        // Validate that the source file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure load options to ignore errors but keep them in the Errors collection
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        try
        {
            // Load the DWFx file as a CadImage
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                // Set up rasterization options with a render‑result callback
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1200,
                    PageHeight = 1600,
                    // Callback invoked after rendering
                    RenderResult = result =>
                    {
                        Console.WriteLine($"Render completed: {result.IsRenderComplete}");
                        if (!result.IsRenderComplete && result.Failures != null)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.WriteLine($"Failure: {failure.Message}, Code: {failure.RenderCode}");
                            }
                        }
                    }
                };

                // Configure PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Handle loading exceptions and report collected load errors
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {err.Message}, Code: {err.RenderCode}");
            }
        }
    }
}