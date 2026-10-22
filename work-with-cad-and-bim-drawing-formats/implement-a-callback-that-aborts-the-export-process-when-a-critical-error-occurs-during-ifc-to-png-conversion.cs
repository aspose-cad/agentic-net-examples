using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "model.ifc";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – do not ignore errors so they are reported
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        Image cadImage = null;
        try
        {
            // Load the IFC file
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            // Loading failed – report the exception and any collected errors
            Console.Error.WriteLine($"Failed to load IFC: {ex.Message}");
            foreach (var err in loadOptions.Errors)
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            return;
        }

        // Configure rasterization options with a RenderResult callback
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1000,
            PageHeight = 1000,
            RenderResult = result =>
            {
                // Log all rendering failures
                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
                }

                // If any failure is considered critical, abort the export
                if (result.Failures.Count > 0)
                {
                    // Throwing an exception aborts the Save operation
                    throw new InvalidOperationException("Critical rendering error encountered – aborting export.");
                }
            }
        };

        // Set PNG export options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            // Perform the export
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            // Export was aborted or failed – report the reason
            Console.Error.WriteLine($"Export aborted: {ex.Message}");
        }
        finally
        {
            // Clean up resources
            cadImage?.Dispose();
        }
    }
}