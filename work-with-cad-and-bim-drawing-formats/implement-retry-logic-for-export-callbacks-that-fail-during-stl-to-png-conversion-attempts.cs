using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class StlToPngWithRetry
{
    static void Main()
    {
        // Path to the source STL file
        const string inputPath = "model.stl";
        // Path for the output PNG file
        const string outputPath = "model.png";

        // Verify input file exists
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
            IgnoreErrors = false // we want to capture load errors
        };

        Image cadImage = null;
        try
        {
            // Load the STL file
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load file: {ex.Message}");
            // Inspect load errors if any
            foreach (var loadError in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {loadError.RenderCode}: {loadError.Message}");
            }
            return;
        }

        // Retry configuration
        const int maxRetries = 3;
        int attempt = 0;
        bool exportSucceeded = false;

        while (attempt < maxRetries && !exportSucceeded)
        {
            attempt++;
            bool hasRenderFailures = false;

            // Configure rasterization options with a RenderResult callback
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1000,
                PageHeight = 1000,
                RenderResult = result =>
                {
                    // Check for failures after each render pass
                    foreach (var failure in result.Failures)
                    {
                        Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
                        hasRenderFailures = true;
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
                // Attempt to save the PNG
                cadImage.Save(outputPath, pngOptions);

                // If the callback reported no failures, consider the export successful
                if (!hasRenderFailures)
                {
                    exportSucceeded = true;
                    Console.WriteLine($"Export succeeded on attempt {attempt}.");
                }
                else
                {
                    Console.WriteLine($"Export attempt {attempt} completed with render failures; retrying...");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Save failed on attempt {attempt}: {ex.Message}");
                // Continue to next attempt
            }
        }

        if (!exportSucceeded)
        {
            Console.Error.WriteLine($"Export failed after {maxRetries} attempts.");
        }

        // Clean up
        cadImage?.Dispose();
    }
}