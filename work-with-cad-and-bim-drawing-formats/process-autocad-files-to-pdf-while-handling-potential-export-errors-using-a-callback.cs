using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input AutoCAD file (DWG, DXF, etc.)
        string sourcePath = "input.dwg";
        // Desired PDF output path
        string outputPath = "output.pdf";

        // Guard against missing source file
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure load options – collect errors without throwing immediately
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;
        try
        {
            // Load the CAD document using the prepared options
            cadImage = (CadImage)Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            // Loading failed – report exception and any collected load errors
            Console.Error.WriteLine($"Load exception: {ex.Message}");
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {err.RenderCode}: {err.Message}");
            }
            return;
        }

        // Prepare PDF export options with rasterization settings and error callback
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = result =>
            {
                Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                }
            }
        };

        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Export the CAD image to PDF using the configured options
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            // Saving failed – report exception
            Console.Error.WriteLine($"Save exception: {ex.Message}");
        }
        finally
        {
            // Release resources
            cadImage?.Dispose();
        }
    }
}