using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.svg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
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
            Console.WriteLine($"Failed to load SVG: {ex.Message}");
            foreach (var err in loadOptions.Errors)
                Console.WriteLine($"Load error: {err.Message} ({err.RenderCode})");
            return;
        }

        if (loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load completed with warnings:");
            foreach (var err in loadOptions.Errors)
                Console.WriteLine($"Warning: {err.Message} ({err.RenderCode})");
        }

        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 1.0f,
            PageWidth = 800,
            PageHeight = 600,
            Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
        };

        rasterOptions.RenderResult = result =>
        {
            if (result.IsRenderComplete)
            {
                Console.WriteLine("Render completed successfully.");
            }
            else
            {
                Console.WriteLine("Render failed.");
                if (result.Failures != null)
                {
                    foreach (var fail in result.Failures)
                        Console.WriteLine($"  Failure: {fail.Message} ({fail.RenderCode})");
                }
            }
        };

        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine("SVG exported to PDF successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save PDF: {ex.Message}");
        }
    }
}