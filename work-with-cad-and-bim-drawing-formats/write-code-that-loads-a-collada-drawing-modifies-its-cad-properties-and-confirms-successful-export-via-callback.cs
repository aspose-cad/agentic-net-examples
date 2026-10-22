using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadColladaExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input COLLADA file path
            string inputPath = "input.dae";
            // Output PNG file path
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

            // Configure load options
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the CAD image
                using (Image image = Image.Load(inputPath, loadOptions))
                {
                    // Access CAD-specific members
                    if (image is CadImage cadImage)
                    {
                        // Example modification: set default font
                        cadImage.DefaultFont = "Arial";
                    }

                    // Set up rasterization options for PNG export
                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 0.5f,
                        PageWidth = 800,
                        PageHeight = 600,
                        AutomaticLayoutsScaling = false,
                        RenderResult = result =>
                        {
                            // Callback provides render status and any failures
                            Console.WriteLine($"Render callback - IsComplete: {result.IsRenderComplete}");
                            if (result.Failures != null && result.Failures.Count > 0)
                            {
                                foreach (var failure in result.Failures)
                                {
                                    Console.WriteLine($"Failure - Message: {failure.Message}, Code: {failure.RenderCode}");
                                }
                            }
                        }
                    };

                    // Prepare PNG save options and attach rasterization settings
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image to PNG format
                    image.Save(outputPath, pngOptions);
                    Console.WriteLine($"Export completed successfully: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                // Handle loading exceptions and report any load errors collected in LoadOptions
                Console.WriteLine($"Error loading CAD image: {ex.Message}");
                foreach (var loadError in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error - Message: {loadError.Message}, Code: {loadError.RenderCode}");
                }
            }
        }
    }
}