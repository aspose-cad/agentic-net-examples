using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadLoadErrorHandler
{
    static void Main()
    {
        try
        {
            // Input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Configure load options to ignore errors so that the image can be loaded partially
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the CAD image inside a using block to ensure proper disposal
            using (var cadImage = (CadImage)Image.Load(inputPath, loadOptions))
            {
                // After loading, inspect any errors that occurred during the load process
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Load errors detected:");
                    foreach (var loadError in loadOptions.Errors)
                    {
                        // RenderResult provides Message and RenderCode members
                        Console.WriteLine($"Message: {loadError.Message}, Code: {loadError.RenderCode}");
                    }
                }

                // Set up rasterization options for rendering
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 0.5f,
                    Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };

                // Provide a callback to log render results (e.g., failures) during rasterization
                rasterOptions.RenderResult = result =>
                {
                    // result is Aspose.CAD.ImageOptions.CadRenderResult
                    if (result.IsRenderComplete)
                    {
                        Console.WriteLine("Render completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Render failed. Failures:");
                        foreach (var failure in result.Failures)
                        {
                            Console.WriteLine($"Message: {failure.Message}, Code: {failure.RenderCode}");
                        }
                    }
                };

                // Configure PNG export options and attach the rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Output file path
                string outputPath = "output.png";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine("Processing completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}