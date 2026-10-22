using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class StpToPdfExporter
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "sample.stp";
            string outputPath = "sample.pdf";

            // Guard against missing input file
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

            // LoadOptions with error ignoring enabled
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the CAD image
            Image cadImage = Image.Load(inputPath, loadOptions);

            // Report any load‑time errors captured in LoadOptions.Errors
            foreach (var loadError in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {loadError.Message} (Code: {loadError.RenderCode})");
            }

            // Prepare rasterization options for PDF export
            var rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter,
                LineScale = 0.5f
            };

            // Capture render‑time errors via the RenderResult callback
            var renderResults = new List<CadRenderResult>();
            rasterOptions.RenderResult = result => renderResults.Add(result);

            // Configure PDF export options
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Perform the export
            cadImage.Save(outputPath, pdfOptions);

            // Report any render‑time errors collected by the callback
            foreach (var cadResult in renderResults)
            {
                if (!cadResult.IsRenderComplete && cadResult.Failures != null)
                {
                    foreach (var failure in cadResult.Failures)
                    {
                        Console.WriteLine($"Render error: {failure.Message} (Code: {failure.RenderCode})");
                    }
                }
            }

            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}