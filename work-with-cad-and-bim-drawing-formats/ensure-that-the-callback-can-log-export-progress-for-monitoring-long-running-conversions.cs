using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLoadDiagnosticsAndExportCallback
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";
            // Path to the exported image
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

            // Configure load options to ignore errors and collect them
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the CAD file with the specified options
                using (Image cadImage = Image.Load(inputPath, loadOptions))
                {
                    // Prepare rasterization options with a render callback
                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Millimeter,
                        PageWidth = 800,
                        PageHeight = 600,
                        AutomaticLayoutsScaling = true
                    };

                    // Assign a callback to monitor rendering progress
                    rasterOptions.RenderResult = renderResult =>
                    {
                        Console.WriteLine($"Render progress - Complete: {renderResult.IsRenderComplete}, Failures: {renderResult.Failures?.Count ?? 0}");
                    };

                    // Set up PNG export options using the rasterization settings
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD image to PNG, invoking the render callback
                    cadImage.Save(outputPath, pngOptions);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during loading or saving
                Console.WriteLine($"Exception: {ex.Message}");
            }

            // After the load attempt, output any collected load errors
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.WriteLine("Load diagnostics:");
                foreach (var error in loadOptions.Errors)
                {
                    Console.WriteLine($"- Message: {error.Message}, Code: {error.RenderCode}");
                }
            }
            else
            {
                Console.WriteLine("No load errors reported.");
            }
        }
    }
}