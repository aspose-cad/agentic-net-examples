using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class DwfExportWithErrorCallback
{
    static void Main()
    {
        // Input CAD file path
        string sourcePath = "input.dwg";
        // Output DWF file path
        string outputPath = "output.dwf";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – continue loading even if errors occur
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;
        try
        {
            // Load the CAD image using the specified load options
            cadImage = (CadImage)Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Loading failed: {ex.Message}");
        }

        // Report any load errors collected in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load errors:");
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.Error.WriteLine("CAD image could not be loaded.");
            return;
        }

        // Configure DWF export options with a render result callback
        var dwfOptions = new DwfOptions
        {
            VectorRasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                // Callback to handle rendering diagnostics
                RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.Error.WriteLine("Rendering incomplete.");
                    }

                    if (result.Failures != null && result.Failures.Count > 0)
                    {
                        Console.Error.WriteLine("Render failures:");
                        foreach (var failure in result.Failures)
                        {
                            Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                        }
                    }
                }
            }
        };

        try
        {
            // Save the CAD image as DWF using the configured options
            cadImage.Save(outputPath, dwfOptions);
            Console.WriteLine("Export completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Export failed: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}