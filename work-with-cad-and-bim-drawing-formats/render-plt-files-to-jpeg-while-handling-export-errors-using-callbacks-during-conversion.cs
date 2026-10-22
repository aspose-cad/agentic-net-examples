using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class PltToJpegConverter
{
    static void Main()
    {
        // Paths for input PLT file and output JPEG file
        string sourcePath = "input.plt";
        string outputPath = "output.jpg";

        // Verify input file exists
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

        // Configure load options – ignore errors to allow processing to continue
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;

        // Load the PLT file with the specified options
        try
        {
            cadImage = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load image: {ex.Message}");
            // Report any load errors collected in LoadOptions
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {err.RenderCode}: {err.Message}");
            }
            return;
        }

        // Even if loading succeeded, there may be warnings in LoadOptions.Errors
        if (loadOptions.Errors.Count > 0)
        {
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load warning {err.RenderCode}: {err.Message}");
            }
        }

        // Set up JPEG export options with rasterization settings and a render result callback
        var jpegOptions = new JpegOptions
        {
            VectorRasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.Error.WriteLine("Rendering incomplete.");
                    }

                    foreach (var failure in result.Failures)
                    {
                        Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                    }
                }
            }
        };

        // Save the image as JPEG, handling any export exceptions
        try
        {
            cadImage.Save(outputPath, jpegOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save JPEG: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}