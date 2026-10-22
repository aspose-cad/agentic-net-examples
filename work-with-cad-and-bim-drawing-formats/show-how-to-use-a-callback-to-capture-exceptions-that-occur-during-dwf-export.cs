using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class DwfExportWithDiagnostics
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dwf";

        // Verify input file exists
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

        // Prepare load options – enable ignoring errors so the load can continue.
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;

        // Load the CAD file inside a try/catch block.
        try
        {
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Load exception: {ex.Message}");
            return;
        }

        // After loading, inspect any non‑critical errors that were collected.
        if (loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load warnings / errors:");
            foreach (var warning in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{warning.RenderCode}: {warning.Message}");
            }
        }

        // Configure DWF export options with a render result callback.
        var dwfOptions = new DwfOptions
        {
            VectorRasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                RenderResult = result =>
                {
                    Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                    foreach (var failure in result.Failures)
                    {
                        Console.Error.WriteLine($"Render failure - {failure.RenderCode}: {failure.Message}");
                    }
                }
            }
        };

        // Save the CAD image to DWF, capturing any exceptions that may be thrown.
        try
        {
            cadImage.Save(outputPath, dwfOptions);
            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Save exception: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}