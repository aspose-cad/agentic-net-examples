using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input COLLADA file and output PDF paths
        string inputPath = "drawing.dae";
        string outputPath = "drawing.pdf";

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

        // Prepare load options
        var loadOptions = new LoadOptions();

        Image cadImage = null;
        try
        {
            // Load the COLLADA drawing
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load image: {ex.Message}");
            // Report any load errors collected in LoadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {err.RenderCode}: {err.Message}");
            }
            return;
        }

        // Flag to capture render success reported by the callback
        bool renderSuccess = false;

        // Configure rasterization options with a render result callback
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = result =>
            {
                if (result.IsRenderComplete && result.Failures.Count == 0)
                {
                    renderSuccess = true;
                }
                else
                {
                    foreach (RenderResult failure in result.Failures)
                    {
                        Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                    }
                }
            }
        };

        // Set up PDF export options
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Save the CAD image as PDF; the RenderResult callback will be invoked here
            cadImage.Save(outputPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save PDF: {ex.Message}");
            return;
        }

        // Verify the callback indicated success
        if (renderSuccess)
        {
            Console.WriteLine("PDF export completed successfully; render callback reported success.");
        }
        else
        {
            Console.WriteLine("PDF export completed, but the render callback reported failures.");
        }
    }
}