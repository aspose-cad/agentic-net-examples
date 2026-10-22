using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = args.Length > 0 ? args[0] : "input.iges";
        string outputPath = args.Length > 1 ? args[1] : "output.pdf";

        ExportIgesToPdf(inputPath, outputPath);
    }

    public static void ExportIgesToPdf(string inputPath, string outputPath)
    {
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        string outDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        try
        {
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Load errors:");
                    foreach (var err in loadOptions.Errors)
                    {
                        Console.WriteLine($"  {err.Message} (Code: {err.RenderCode})");
                    }
                }

                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 0.5f,
                    Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };

                rasterOptions.RenderResult = (CadRenderResult renderResult) =>
                {
                    if (!renderResult.IsRenderComplete)
                    {
                        Console.WriteLine("Render incomplete.");
                    }

                    if (renderResult.Failures != null && renderResult.Failures.Count > 0)
                    {
                        Console.WriteLine("Render failures:");
                        foreach (var failure in renderResult.Failures)
                        {
                            Console.WriteLine($"  {failure.Message} (Code: {failure.RenderCode})");
                        }
                    }
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}