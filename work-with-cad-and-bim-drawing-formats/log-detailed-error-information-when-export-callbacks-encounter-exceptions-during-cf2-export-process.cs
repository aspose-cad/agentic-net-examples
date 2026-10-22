using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input and output file paths
        string inputPath = "input.dwg";
        string outputPath = "output.cf2";

        // Guard against missing input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure load options
        var loadOptions = new LoadOptions();
        loadOptions.IgnoreErrors = true;

        CadImage cadImage = null;
        try
        {
            // Load the CAD file with the specified load options
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load CAD file: " + ex.Message);
            // Log any errors captured during loading
            foreach (var error in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {error.Message} (Code: {error.RenderCode})");
            }
            return;
        }

        // Log warnings or non‑critical errors that were captured
        foreach (var warning in loadOptions.Errors)
        {
            Console.WriteLine($"Load warning: {warning.Message} (Code: {warning.RenderCode})");
        }

        // Set up rasterization options with a render result callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Millimeter,
            PageWidth = 800,
            PageHeight = 600,
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.WriteLine("Render incomplete. Failures:");
                    foreach (var failure in result.Failures)
                    {
                        Console.WriteLine($"  {failure.Message} (Code: {failure.RenderCode})");
                    }
                }
            }
        };

        // Attempt to export to CF2 format
        // Note: Aspose.CAD SDK does not provide a CF2 export option.
        // Therefore, detailed export callbacks cannot be demonstrated for CF2.
        // The following block logs this limitation.
        if (Path.GetExtension(outputPath).Equals(".cf2", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("CF2 export is not supported by the Aspose.CAD SDK. Unable to perform export.");
        }
        else
        {
            // Placeholder for other supported formats (e.g., Dxf, Pdf, Svg, Png)
            try
            {
                // Example using DxfOptions for a supported format
                var dxfOptions = new DxfOptions
                {
                    VectorRasterizationOptions = rasterOptions,
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Export failed: " + ex.Message);
            }
        }

        // Ensure resources are released
        cadImage?.Dispose();
    }
}