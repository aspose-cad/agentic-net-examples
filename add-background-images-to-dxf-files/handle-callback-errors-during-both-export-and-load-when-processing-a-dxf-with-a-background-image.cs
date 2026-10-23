using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dxf";
        string destinationPath = "output.pdf";

        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Prepare load options to capture errors
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        CadImage cadImage = null;
        try
        {
            // Load the DXF file
            cadImage = (CadImage)Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load DXF file: {ex.Message}");
            // Report any load errors collected in LoadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
            return;
        }

        // Even if no exception, there may be load warnings/errors
        if (loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load completed with the following messages:");
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
        }

        // Ensure output directory exists
        string outDir = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        // Set up PDF export options with a render result callback
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 800,
                PageHeight = 600,
                RenderResult = result =>
                {
                    // Report any rendering failures
                    foreach (var failure in result.Failures)
                    {
                        Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                    }

                    if (result.IsRenderComplete)
                    {
                        Console.WriteLine("Render completed successfully.");
                    }
                }
            }
        };

        try
        {
            // Export the CAD drawing to PDF
            cadImage.Save(destinationPath, pdfOptions);
            Console.WriteLine("DXF successfully exported to PDF.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to export PDF: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}