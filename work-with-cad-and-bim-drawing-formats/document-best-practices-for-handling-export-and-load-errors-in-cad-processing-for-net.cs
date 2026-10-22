using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadProcessingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            // Guard input file existence
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

            // Load options
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            CadImage cadImage;
            try
            {
                cadImage = (CadImage)Image.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error loading CAD file: {ex.Message}");
                return;
            }

            // Report non‑fatal load errors
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.WriteLine("Non‑fatal load errors detected:");
                foreach (var error in loadOptions.Errors)
                {
                    Console.WriteLine($"- {error.Message} (Code: {error.RenderCode})");
                }
            }

            // Rasterization options
            var rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Millimeter,
                PageWidth = 1600,
                PageHeight = 1200,
                AutomaticLayoutsScaling = true,
                Layouts = new[] { "Model" }
            };

            // Render result callback – detailed result fields are not available in this SDK version
            rasterOptions.RenderResult = result =>
            {
                Console.WriteLine("Render result callback invoked. Detailed result information is not available in this SDK version.");
            };

            // PNG export options
            var pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export
            try
            {
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine("Export completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during export: {ex.Message}");
            }
        }
    }
}