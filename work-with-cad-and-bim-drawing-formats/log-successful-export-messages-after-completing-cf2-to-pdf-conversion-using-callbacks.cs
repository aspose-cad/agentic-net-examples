using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

public class CadToPdfConverter
{
    public static void Convert(string inputPath, string outputPath)
    {
        // Verify that the source file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Load the CAD file with error‑ignoring options
        var loadOptions = new LoadOptions { IgnoreErrors = true };
        CadImage? cadImage = null;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            // Log any load errors collected in LoadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {err.Message}, Code: {err.RenderCode}");
            }
            return;
        }

        // Configure rasterization options and attach a render‑result callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 0.5f
        };

        // Callback invoked after rendering; logs success when there are no failures
        rasterOptions.RenderResult = result =>
        {
            if (result.IsRenderComplete && (result.Failures == null || result.Failures.Count == 0))
            {
                Console.WriteLine("CAD rendering completed successfully.");
            }
            else if (result.Failures != null)
            {
                foreach (var failure in result.Failures)
                {
                    Console.WriteLine($"Render failure: {failure.Message}, Code: {failure.RenderCode}");
                }
            }
        };

        // Set up PDF export options using the rasterization settings
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the CAD image as PDF and log the outcome
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"PDF saved successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save PDF: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }

    // Example entry point
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: CadToPdfConverter <input.cad> <output.pdf>");
            return;
        }

        Convert(args[0], args[1]);
    }
}