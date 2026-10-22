using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.sat";
        string outputPath = "output.png";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        // Configure load options
        var loadOptions = new LoadOptions
        {
            // Do not ignore errors so they are collected in loadOptions.Errors
            IgnoreErrors = false
        };

        Image cadImage = null;
        try
        {
            // Load the SAT file as CadImage
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load image: {ex.Message}");
            // Report any load errors captured in loadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error [{err.RenderCode}]: {err.Message}");
            }
            return;
        }

        // Set up rasterization options with a render result callback
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = result =>
            {
                // Overall render status
                Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                // Detailed failures, if any
                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"Render failure [{failure.RenderCode}]: {failure.Message}");
                }
            }
        };

        // Configure PNG export options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Export to PNG
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save image: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}