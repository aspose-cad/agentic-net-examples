using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input FBX file and output PDF file paths
        string inputPath = "sample.fbx";
        string outputPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – do not ignore errors so they are collected
        LoadOptions loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        Image cadImage = null;

        // Load the FBX drawing and capture any load exceptions
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Load exception: {ex.Message}");
        }

        // Report load‑time errors collected in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {err.RenderCode}: {err.Message}");
            }
        }

        // Proceed only if the image was successfully loaded
        if (cadImage != null)
        {
            // Configure PDF export options
            var pdfOptions = new PdfOptions();

            // Rasterization options required for PDF export
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1600
            };

            // Callback to capture rendering diagnostics
            rasterOptions.RenderResult = result =>
            {
                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"Render error {failure.RenderCode}: {failure.Message}");
                }
            };

            pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Save the drawing to PDF and handle any save‑time exceptions
            try
            {
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Save exception: {ex.Message}");
            }
            finally
            {
                cadImage.Dispose();
            }
        }
    }
}