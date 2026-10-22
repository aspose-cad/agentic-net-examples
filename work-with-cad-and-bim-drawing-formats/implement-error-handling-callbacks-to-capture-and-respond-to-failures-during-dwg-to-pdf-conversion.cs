using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class DwgToPdfConverter
{
    static void Main()
    {
        // Input and output file paths
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        // Guard against missing input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure load options to ignore errors and collect them
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        // Load the DWG file
        CadImage cadImage = null;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during load: {ex.Message}");
            return;
        }

        // Report any errors captured during loading
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors:");
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"- {err.Message} (Code: {err.RenderCode})");
            }
        }

        // Set up rasterization options with a render result callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 0.5f
        };

        // Callback invoked after rendering; captures failures if any
        rasterOptions.RenderResult = (CadRenderResult renderResult) =>
        {
            if (!renderResult.IsRenderComplete)
            {
                Console.WriteLine("Render incomplete. Failures:");
                foreach (var failure in renderResult.Failures)
                {
                    Console.WriteLine($"- {failure.Message} (Code: {failure.RenderCode})");
                }
            }
            else
            {
                Console.WriteLine("Render completed successfully.");
            }
        };

        // Configure PDF export options
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Perform the conversion and save to PDF
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"Conversion succeeded. PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during save: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}