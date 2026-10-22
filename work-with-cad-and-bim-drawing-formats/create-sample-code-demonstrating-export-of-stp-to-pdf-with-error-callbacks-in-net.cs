using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class ExportStpToPdf
{
    static void Main()
    {
        // Input STEP file and output PDF paths
        string sourcePath = "sample.stp";
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

        // Prepare load options
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            // Load the STEP file with the specified options
            cadImage = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Load exception: {ex.Message}");
            // Report any load errors collected in LoadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {err.RenderCode}: {err.Message}");
            }
            return;
        }

        // Report any warnings or errors that were recorded during loading
        foreach (var err in loadOptions.Errors)
        {
            Console.Error.WriteLine($"Load warning {err.RenderCode}: {err.Message}");
        }

        // Configure rasterization options for PDF export
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = result =>
            {
                // Enumerate rendering failures, if any
                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
                }

                // Indicate overall rendering status
                if (result.IsRenderComplete)
                {
                    Console.WriteLine("Rendering completed successfully.");
                }
                else
                {
                    Console.WriteLine("Rendering completed with missing elements.");
                }
            }
        };

        // Set PDF export options
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Export the CAD image to PDF
            cadImage.Save(outputPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Save exception: {ex.Message}");
        }
        finally
        {
            // Release resources
            cadImage?.Dispose();
        }
    }
}