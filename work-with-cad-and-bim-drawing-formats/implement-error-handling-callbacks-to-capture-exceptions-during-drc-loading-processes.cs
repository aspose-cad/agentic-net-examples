using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadErrorHandlingExample
{
    static void Main()
    {
        // Input CAD file path
        string inputPath = @"C:\CAD\sample.dwg";

        // Output PNG file path
        string outputPath = @"C:\CAD\Output\sample.png";

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure load options to ignore errors and collect them
        LoadOptions loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        try
        {
            // Load the CAD image with the specified load options
            using (CadImage cadImage = (CadImage)Image.Load(inputPath, loadOptions))
            {
                // After loading, inspect any errors captured by LoadOptions
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Load completed with the following warnings/errors:");
                    foreach (var err in loadOptions.Errors)
                    {
                        Console.WriteLine($"- {err.Message}");
                    }
                }

                // Set up rasterization options for PNG export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example: set unit type and line scale
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // Attach a render result callback to capture rendering issues
                rasterOptions.RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.WriteLine("Rendering did not complete successfully.");
                        if (result.Failures != null)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.WriteLine($"Render failure: {failure.Message}");
                            }
                        }
                    }
                };

                // Configure PNG export options and assign rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as PNG using the configured options
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved successfully to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Capture any exceptions thrown during loading or saving
            Console.WriteLine($"An exception occurred: {ex.Message}");
        }
    }
}