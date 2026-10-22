using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadLoadDiagnostics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the input SAT/SAB file
            string inputPath = "input.sat";
            // Path to the output PNG file
            string outputPath = "output.png";

            // Guard for input file existence
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

            try
            {
                // Load the CAD file with the specified options
                Image cadImage = Image.Load(inputPath, loadOptions);

                // After loading, inspect any errors that were captured
                IList<RenderResult> loadErrors = loadOptions.Errors;
                if (loadErrors != null && loadErrors.Count > 0)
                {
                    Console.WriteLine("Load diagnostics:");
                    foreach (RenderResult error in loadErrors)
                    {
                        // RenderResult provides Message and RenderCode properties
                        Console.WriteLine($"- Code: {error.RenderCode}, Message: {error.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("File loaded without diagnostics.");
                }

                // Prepare rasterization options with a render callback
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Millimeter,
                    PageWidth = 800,
                    PageHeight = 600,
                    AutomaticLayoutsScaling = true,
                    RenderResult = renderResult =>
                    {
                        // RenderResult may not expose detailed properties in this SDK version;
                        // we simply acknowledge the callback invocation.
                        Console.WriteLine("Render callback invoked.");
                    }
                };

                // Set up PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image to PNG, invoking the render callback
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image exported to {outputPath}");
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during loading or saving
                Console.WriteLine($"Exception during CAD load or export: {ex.Message}");
            }
        }
    }
}