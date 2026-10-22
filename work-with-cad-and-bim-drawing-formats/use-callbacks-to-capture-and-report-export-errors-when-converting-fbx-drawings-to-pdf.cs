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
        string sourcePath = "sample.fbx";
        string destPath   = "output.pdf";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outDir = Path.GetDirectoryName(destPath);
        if (!string.IsNullOrEmpty(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        // Configure load options – continue loading even if errors occur
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image image = null;
        try
        {
            // Load the FBX drawing
            image = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Loading failed: {ex.Message}");
            // Report any load‑time errors collected in LoadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
            return;
        }

        // Set up rasterization options for PDF export
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth      = 1600,
            PageHeight     = 1200,
            BackgroundColor = Color.White
        };

        // Callback to capture and report export/render errors
        rasterOptions.RenderResult = result =>
        {
            foreach (var failure in result.Failures)
            {
                Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
            }
        };

        // Configure PDF export options
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Save the drawing as PDF
            image.Save(destPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Saving failed: {ex.Message}");
        }
        finally
        {
            image?.Dispose();
        }
    }
}