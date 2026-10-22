using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class ExportFbxToPdf
{
    static void Main()
    {
        string inputPath = "input.fbx";
        string outputPath = "output.pdf";

        // Guard against missing input file
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

        // Prepare load options
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            // Load the FBX drawing
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load the file: {ex.Message}");
            return;
        }

        // Report any load‑time warnings captured in LoadOptions.Errors
        foreach (var loadError in loadOptions.Errors)
        {
            Console.Error.WriteLine($"Load warning - {loadError.RenderCode}: {loadError.Message}");
        }

        // Configure PDF export options with a render‑result callback
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 1200,
                PageHeight = 800,
                RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.Error.WriteLine("Rendering completed with missing elements.");
                    }

                    foreach (var failure in result.Failures)
                    {
                        Console.Error.WriteLine($"Render warning - {failure.RenderCode}: {failure.Message}");
                    }
                }
            }
        };

        // Save the drawing to PDF
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine("Export to PDF completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save PDF: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}