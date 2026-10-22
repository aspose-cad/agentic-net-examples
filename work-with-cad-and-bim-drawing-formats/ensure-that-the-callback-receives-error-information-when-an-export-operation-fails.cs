using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options with error collection enabled
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;
        try
        {
            // Load the CAD file using the prepared options
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            // Report any exception thrown during loading
            Console.Error.WriteLine($"Load exception: {ex.Message}");
        }

        // Report load‑time errors collected in loadOptions.Errors
        foreach (var loadError in loadOptions.Errors)
        {
            Console.Error.WriteLine($"Load error {loadError.RenderCode}: {loadError.Message}");
        }

        if (cadImage == null)
        {
            Console.Error.WriteLine("Failed to load CAD image.");
            return;
        }

        // Configure rasterization options and attach a RenderResult callback
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200
        };

        rasterOptions.RenderResult = result =>
        {
            // Enumerate all rendering failures (if any) and output details
            foreach (var failure in result.Failures)
            {
                Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
            }

            // Optionally report incomplete rendering
            if (!result.IsRenderComplete)
            {
                Console.Error.WriteLine("Rendering did not complete.");
            }
        };

        // Set up PNG export options using the rasterization settings
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the image and capture any save‑time exceptions
        try
        {
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Save exception: {ex.Message}");
        }
    }
}