using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadToPdfWithErrorCallback
{
    static void Main()
    {
        string inputPath = "sample.cf2";
        string outputPath = "output/result.pdf";

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

        try
        {
            var cadImage = Image.Load(inputPath, loadOptions) as CadImage;
            if (cadImage == null)
            {
                Console.Error.WriteLine("Failed to load CAD image.");
                return;
            }

            foreach (var loadError in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error [{loadError.RenderCode}]: {loadError.Message}");
            }

            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1200,
                PageHeight = 800,
                RenderResult = result =>
                {
                    foreach (var failure in result.Failures)
                    {
                        Console.Error.WriteLine($"Render error [{failure.RenderCode}]: {failure.Message}");
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
                Console.WriteLine("PDF saved successfully.");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Save failed: {saveEx.Message}");
            }
        }
        catch (Exception loadEx)
        {
            Console.Error.WriteLine($"Load failed: {loadEx.Message}");
        }
    }
}