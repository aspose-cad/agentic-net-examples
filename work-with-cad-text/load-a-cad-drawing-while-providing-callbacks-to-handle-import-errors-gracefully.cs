using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class LoadCadWithErrorHandling
{
    static void Main()
    {
        // Path to the CAD file
        string cadFilePath = "sample.dwg";

        // Verify that the file exists before attempting to load
        if (!File.Exists(cadFilePath))
        {
            Console.WriteLine($"File not found: {cadFilePath}");
            return;
        }

        // Configure load options to ignore errors and collect them
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;

        try
        {
            // Load the CAD drawing using the configured options
            cadImage = (CadImage)Image.Load(cadFilePath, loadOptions);
        }
        catch (Exception ex)
        {
            // Handle unexpected load exceptions
            Console.WriteLine($"Exception while loading CAD file: {ex.Message}");
            return;
        }

        // Process any errors that occurred during loading
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Errors encountered during CAD import:");
            foreach (var err in loadOptions.Errors)
            {
                // RenderResult provides Message and RenderCode properties
                Console.WriteLine($"- {err.Message} (Code: {err.RenderCode})");
            }
        }
        else
        {
            Console.WriteLine("CAD file loaded without import errors.");
        }

        // Prepare rasterization options with a render callback to capture export errors
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 0.5f
        };

        // Assign a callback that will be invoked after rendering
        rasterOptions.RenderResult = result =>
        {
            // If there are any failures, report them
            if (result.Failures != null && result.Failures.Count > 0)
            {
                Console.WriteLine("Errors encountered during rendering:");
                foreach (var failure in result.Failures)
                {
                    Console.WriteLine($"- {failure.Message} (Code: {failure.RenderCode})");
                }
            }
            else
            {
                Console.WriteLine("Rendering completed successfully.");
            }
        };

        // Set up PNG export options and link the rasterization options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Define output file path
        string outputPath = "output.png";

        // Save the CAD drawing to PNG, invoking the render callback
        cadImage.Save(outputPath, pngOptions);

        Console.WriteLine($"CAD drawing saved to {outputPath}");
    }
}