using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class PltToPdfWithErrorCallback
{
    static void Main()
    {
        string inputPath = "input.plt";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Load exception: {ex.Message}");
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
            return;
        }

        foreach (var err in loadOptions.Errors)
        {
            Console.Error.WriteLine($"Load warning {err.RenderCode}: {err.Message}");
        }

        var pdfOptions = new PdfOptions();
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1600
        };

        rasterOptions.RenderResult = result =>
        {
            if (!result.IsRenderComplete)
            {
                Console.Error.WriteLine("Rendering completed with missing elements.");
            }

            foreach (var failure in result.Failures)
            {
                Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
            }
        };

        pdfOptions.VectorRasterizationOptions = rasterOptions;

        try
        {
            cadImage.Save(outputPath, pdfOptions);
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