using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input STL file and output PNG file paths
        string inputPath = "sample.stl";
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

        // Configure load options (ignore errors to continue processing)
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        // Load the STL file as a CadImage
        Image cadImage;
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load the CAD file: " + ex.Message);
            // Inspect any load errors collected in LoadOptions
            foreach (var error in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {error.Message} (Code: {error.RenderCode})");
            }
            return;
        }

        // Set up rasterization options with a render callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Millimeter,
            PageWidth = 800,
            PageHeight = 600,
            Layouts = new[] { "Model" },
            RenderResult = result =>
            {
                Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                if (!result.IsRenderComplete)
                {
                    foreach (var failure in result.Failures)
                    {
                        Console.WriteLine($"Render failure: {failure.Message} (Code: {failure.RenderCode})");
                    }
                }
            }
        };

        // Configure PNG export options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the image as PNG; the callback will be invoked during rendering
        try
        {
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to save PNG: " + ex.Message);
        }
        finally
        {
            cadImage.Dispose();
        }

        Console.WriteLine("STL to PNG conversion finished.");
    }
}