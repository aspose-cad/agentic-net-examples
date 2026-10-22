using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class SatSabExportWithErrorHandling
{
    static void Main()
    {
        const string inputPath = @"C:\CAD\sample.sat";
        const string outputPath = @"C:\CAD\output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage;
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load CAD file: {ex.Message}");
            return;
        }

        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load‑time structure errors detected:");
            foreach (var error in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{error.RenderCode}: {error.Message}");
            }
        }

        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.Error.WriteLine("Rendering did not complete successfully.");
                }

                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                }
            }
        };

        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save PNG: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}