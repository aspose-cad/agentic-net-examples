using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class IgesToPdfExporter
{
    static void Main(string[] args)
    {
        // Input IGES file path
        string inputPath = @"C:\CAD\sample.igs";

        // Output PDF file path
        string outputPath = @"C:\CAD\output\sample.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options (errors are collected in LoadOptions.Errors)
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        Image image = null;
        try
        {
            // Load the IGES file
            image = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception while loading file: {ex.Message}");
        }

        // Report any load errors collected in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load errors:");
            foreach (RenderResult err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
        }

        if (image == null)
        {
            Console.Error.WriteLine("Failed to load image.");
            return;
        }

        // Configure PDF export options with a rasterization callback
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1600,
                RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.Error.WriteLine("Rendering completed with issues.");
                    }

                    foreach (RenderResult failure in result.Failures)
                    {
                        Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                    }
                }
            }
        };

        try
        {
            // Save the image as PDF
            image.Save(outputPath, pdfOptions);
            Console.WriteLine($"Export succeeded: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception during export: {ex.Message}");
        }
    }
}