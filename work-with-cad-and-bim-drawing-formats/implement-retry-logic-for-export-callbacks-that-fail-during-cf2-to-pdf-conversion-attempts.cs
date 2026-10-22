using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadToPdfRetryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example usage – replace with actual file paths as needed.
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            try
            {
                CadExporter.Export(inputPath, outputPath);
                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }
    }

    public static class CadExporter
    {
        // Exports a CAD file to PDF, retrying the rasterization if it fails.
        // maxRetries – maximum number of render attempts (default 3).
        public static void Export(string inputFilePath, string outputFilePath, int maxRetries = 3)
        {
            // Validate input file.
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException($"Input CAD file not found: {inputFilePath}");

            // Ensure output directory exists.
            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load CAD image with error‑ignoring enabled.
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the CAD image.
            using (var cadImage = (CadImage)Image.Load(inputFilePath, loadOptions))
            {
                // After loading, inspect any non‑critical errors.
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Non‑critical load errors:");
                    foreach (var err in loadOptions.Errors)
                    {
                        Console.WriteLine($"  - {err.GetType().Name}");
                    }
                }

                int attempt = 0;
                bool renderSucceeded = false;

                while (attempt < maxRetries && !renderSucceeded)
                {
                    attempt++;
                    bool renderFailed = false;

                    // Configure rasterization options.
                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                        // Note: The Aspose.CAD SDK does not expose a render‑callback delegate
                        // (e.g., CadRenderHandler) for CadRasterizationOptions, so callback‑based
                        // retry detection is not available. Retry logic therefore relies on
                        // catching exceptions thrown during the Save operation.
                    };

                    // Configure PDF export options and link raster options.
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    try
                    {
                        // Perform the save operation (this triggers rendering).
                        cadImage.Save(outputFilePath, pdfOptions);
                    }
                    catch (Exception ex)
                    {
                        // Save can throw if rendering fails catastrophically.
                        Console.WriteLine($"Exception during save on attempt {attempt}: {ex.Message}");
                        renderFailed = true;
                    }

                    renderSucceeded = !renderFailed;

                    if (!renderSucceeded && attempt < maxRetries)
                    {
                        Console.WriteLine($"Render attempt {attempt} failed. Retrying...");
                    }
                }

                if (!renderSucceeded)
                {
                    throw new InvalidOperationException($"Failed to render CAD file to PDF after {maxRetries} attempts.");
                }
            }
        }
    }
}