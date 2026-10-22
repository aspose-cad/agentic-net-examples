using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "model.stl";
        string outputPath = "output.png";

        // Verify input file exists
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

        // Prepare load options
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            // Inspect load errors if any
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {err.Message} (Code: {err.RenderCode})");
            }
            return;
        }

        // Check for warnings after successful load
        if (loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load completed with warnings:");
            foreach (var warning in loadOptions.Errors)
            {
                Console.WriteLine($"Warning: {warning.Message} (Code: {warning.RenderCode})");
            }
        }

        // Configure rasterization options with export callback
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 800f,
            PageHeight = 600f,
            UnitType = UnitType.Centimenter,
            RenderResult = result =>
            {
                if (result.IsRenderComplete)
                {
                    Console.WriteLine("Render completed successfully.");
                }
                else
                {
                    Console.WriteLine("Render incomplete. Failures:");
                    if (result.Failures != null)
                    {
                        foreach (var failure in result.Failures)
                        {
                            Console.WriteLine($"Failure: {failure.Message} (Code: {failure.RenderCode})");
                        }
                    }
                }
            }
        };

        // Set PNG export options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the image
        try
        {
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save PNG: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}