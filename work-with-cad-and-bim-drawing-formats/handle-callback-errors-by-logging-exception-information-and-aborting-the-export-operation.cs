using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        // Verify input file exists
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

        // Load CAD image with error handling
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        Image cadImage = null;
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during loading: {ex.Message}");
            return;
        }

        // Check for load-time errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors detected:");
            foreach (var err in loadOptions.Errors)
            {
                // RenderResult has Message property
                Console.WriteLine($"- {err.Message}");
            }
            // Abort export due to load errors
            return;
        }

        // Configure rasterization options with a render callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            PageWidth = 800,
            PageHeight = 600,
            // Callback invoked after rendering
            RenderResult = result =>
            {
                // Log callback invocation
                Console.WriteLine("Render callback invoked.");

                // Attempt to log message if available
                var msgProp = result?.GetType().GetProperty("Message");
                if (msgProp != null)
                {
                    var msg = msgProp.GetValue(result) as string;
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Console.WriteLine($"Render message: {msg}");
                    }
                }

                // Abort the export by throwing an exception
                throw new InvalidOperationException("Rendering was not successful.");
            }
        };

        // Set up PNG export options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Perform export
        try
        {
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine("Export completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export aborted: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}