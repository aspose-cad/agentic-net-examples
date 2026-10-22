using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace CadExportRetryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            // Guard against missing input file
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Configure load options to ignore errors and collect them
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Attempt to load the CAD file and inspect any load errors
            CadImage cadImage;
            try
            {
                cadImage = (CadImage)Image.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load CAD file: {ex.Message}");
                // Inspect collected errors
                foreach (var err in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {err.Message} (Code: {err.RenderCode})");
                }
                return;
            }

            // Ensure the image is disposed after processing
            using (cadImage)
            {
                const int maxRetries = 3;
                int attempt = 0;
                bool exportSucceeded = false;

                while (attempt < maxRetries && !exportSucceeded)
                {
                    attempt++;
                    Console.WriteLine($"Export attempt {attempt}...");

                    // Flags to capture rendering outcome
                    bool renderComplete = false;
                    bool transientFailure = false;

                    // Configure rasterization options
                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Millimeter,
                        PageWidth = 800,
                        PageHeight = 600,
                        AutomaticLayoutsScaling = true,
                        RenderResult = result =>
                        {
                            renderComplete = result.IsRenderComplete;
                            if (!result.IsRenderComplete)
                            {
                                // Treat any incomplete render as transient for retry purposes
                                transientFailure = true;
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
                        // Perform the export
                        cadImage.Save(outputPath, pngOptions);

                        // Check the callback flags
                        if (renderComplete)
                        {
                            Console.WriteLine("Export completed successfully.");
                            exportSucceeded = true;
                        }
                        else if (transientFailure)
                        {
                            Console.WriteLine("Transient rendering failure detected, will retry.");
                        }
                        else
                        {
                            Console.WriteLine("Rendering failed with non‑transient error, aborting.");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception during export: {ex.Message}");
                        // Retry on exception if attempts remain
                    }
                }

                if (!exportSucceeded)
                {
                    Console.WriteLine("Export failed after maximum retry attempts.");
                }
            }
        }
    }
}