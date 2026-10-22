using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportWithErrorCallback
{
    class Program
    {
        static void Main()
        {
            // Input CAD file path
            string inputPath = "sample.dwg";

            // Verify that the source file exists before attempting to load it
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare load options – ignore errors during loading so that the process can continue
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            CadImage cadImage = null;
            try
            {
                // Load the CAD drawing with the specified options
                cadImage = (CadImage)Image.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                // Loading failed catastrophically; report and exit
                Console.Error.WriteLine($"Failed to load CAD file: {ex.Message}");
                return;
            }

            // After loading, inspect any non‑fatal errors collected in LoadOptions.Errors
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.Error.WriteLine("Load-time errors:");
                foreach (var err in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
                }
            }

            // Configure rasterization options with a callback to capture rendering errors
            var rasterOptions = new CadRasterizationOptions
            {
                // Example: set desired page size and resolution
                PageWidth = 1600,
                PageHeight = 1200,
                Layouts = new[] { "Model" },
                // Callback invoked after each rendering pass
                RenderResult = result =>
                {
                    // Report any failures that occurred during rendering
                    if (result.Failures != null && result.Failures.Count > 0)
                    {
                        Console.Error.WriteLine("Render-time failures:");
                        foreach (var failure in result.Failures)
                        {
                            Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                        }
                    }

                    // Optionally, note whether rendering completed successfully
                    Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                }
            };

            // Prepare export options.
            // Aspose.CAD does not provide a DwfExportOptions class; using DxfOptions as a vector export example.
            var exportOptions = new DxfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Output file path
            string outputPath = "exported_output.dxf";

            try
            {
                // Save the CAD image using the configured export options
                cadImage.Save(outputPath, exportOptions);
                Console.WriteLine($"Export completed successfully: {outputPath}");
            }
            catch (Exception ex)
            {
                // Capture any exceptions thrown during the save operation
                Console.Error.WriteLine($"Export failed: {ex.Message}");
            }
            finally
            {
                // Release resources held by the CadImage instance
                cadImage?.Dispose();
            }
        }
    }
}