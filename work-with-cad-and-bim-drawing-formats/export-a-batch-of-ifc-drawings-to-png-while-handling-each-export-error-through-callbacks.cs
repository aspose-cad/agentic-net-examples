using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // List of IFC files to process
        var ifcFiles = new List<string>
        {
            "drawing1.ifc",
            "drawing2.ifc"
            // add additional file paths as needed
        };

        foreach (var filePath in ifcFiles)
        {
            Console.WriteLine($"Processing: {filePath}");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Input file not found: {filePath}");
                continue;
            }

            // LoadOptions with error ignoring enabled
            var loadOptions = new LoadOptions { IgnoreErrors = true };

            CadImage cadImage = null;
            try
            {
                // Load the IFC drawing
                cadImage = (CadImage)Image.Load(filePath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load exception for {filePath}: {ex.Message}");
                continue;
            }

            // Report any load errors captured in LoadOptions
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                foreach (var err in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {err.Message} ({err.RenderCode})");
                }
            }

            // Configure rasterization options with a render callback
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                UnitType = UnitType.Centimenter,
                RenderResult = result =>
                {
                    // Use available properties of RenderResult
                    Console.WriteLine($"Render completed: {result.IsRenderComplete}");
                    if (result.Failures != null && result.Failures.Count > 0)
                    {
                        foreach (var failure in result.Failures)
                        {
                            Console.WriteLine($"Render failure: {failure.Message} ({failure.RenderCode})");
                        }
                    }
                }
            };

            // Set PNG export options
            var pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Determine output PNG path and ensure directory exists
            var outputPath = Path.ChangeExtension(filePath, ".png");
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Export to PNG
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Saved PNG: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export exception for {filePath}: {ex.Message}");
            }
            finally
            {
                cadImage?.Dispose();
            }
        }
    }
}