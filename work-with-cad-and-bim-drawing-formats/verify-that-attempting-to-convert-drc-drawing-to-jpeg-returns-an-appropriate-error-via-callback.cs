using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.drc";
        string outputPath = "output.jpg";

        // Verify that the source file exists before attempting any I/O.
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists.
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – do not ignore errors so they are collected.
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        CadImage? cadImage = null;
        try
        {
            // Load the DRC drawing.
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loading failed: {ex.Message}");
            // Report any errors captured by LoadOptions.
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {err.Message}");
            }
            return;
        }

        // Configure rasterization options with a render‑result callback.
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 800,
            PageHeight = 600,
            UnitType = UnitType.Centimenter,
            RenderResult = result =>
            {
                // The callback receives CadRenderResult.
                if (!result.IsRenderComplete)
                {
                    Console.WriteLine("Render completed with failures:");
                    foreach (var failure in result.Failures)
                    {
                        Console.WriteLine($"  Failure: {failure.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Render completed successfully.");
                }
            }
        };

        // JPEG export options that reference the rasterization settings.
        var jpegOptions = new JpegOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Attempt to convert the DRC drawing to JPEG.
            // The render callback should report the appropriate error.
            cadImage.Save(outputPath, jpegOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Saving failed: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}