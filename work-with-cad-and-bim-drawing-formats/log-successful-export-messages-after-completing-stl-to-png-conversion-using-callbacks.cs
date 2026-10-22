using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class StlToPngConverter
{
    static void Main()
    {
        // Input STL file path
        string inputPath = "sample.stl";
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

        // Configure load options to ignore errors but capture them
        var loadOptions = new LoadOptions { IgnoreErrors = true };

        try
        {
            // Load the STL file as a CadImage
            using (var cadImage = Image.Load(inputPath, loadOptions))
            {
                // Set up rasterization options with a render callback
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // Callback invoked after rendering
                rasterOptions.RenderResult = result =>
                {
                    if (result.IsRenderComplete)
                    {
                        Console.WriteLine("Export completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Export incomplete. Failures count: {result.Failures.Count}");
                        foreach (var failure in result.Failures)
                        {
                            // RenderResult does not expose a Message property; use ToString for details
                            Console.WriteLine($"Failure detail: {failure}");
                        }
                    }
                };

                // Configure PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image as PNG
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
            // Report any load errors captured in LoadOptions
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.WriteLine("Load errors:");
                foreach (var err in loadOptions.Errors)
                {
                    Console.WriteLine(err);
                }
            }
        }
    }
}