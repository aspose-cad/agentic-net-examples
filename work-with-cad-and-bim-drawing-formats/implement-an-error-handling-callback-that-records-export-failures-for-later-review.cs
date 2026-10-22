using System;
using System.Collections.Generic;
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

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
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

        CadImage cadImage = null;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading CAD file: {ex.Message}");
        }

        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load errors:");
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
        }

        var exportFailures = new List<RenderResult>();

        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = cadResult =>
            {
                if (cadResult.Failures != null && cadResult.Failures.Count > 0)
                {
                    exportFailures.AddRange(cadResult.Failures);
                    foreach (var failure in cadResult.Failures)
                    {
                        Console.Error.WriteLine($"Export failure – {failure.RenderCode}: {failure.Message}");
                    }
                }
            }
        };

        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        if (cadImage != null)
        {
            try
            {
                cadImage.Save(outputPath, pngOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving raster image: {ex.Message}");
            }
        }

        if (exportFailures.Count > 0)
        {
            Console.WriteLine($"Total export failures recorded: {exportFailures.Count}");
        }

        cadImage?.Dispose();
    }
}