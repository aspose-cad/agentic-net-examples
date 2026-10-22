using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "drawing.fbx";
        string outputPath = "drawing.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
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
        }

        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            foreach (RenderResult loadError in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {loadError.RenderCode}: {loadError.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.Error.WriteLine("Failed to load the FBX drawing.");
            return;
        }

        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200
        };

        rasterOptions.RenderResult = result =>
        {
            foreach (RenderResult failure in result.Failures)
            {
                Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
            }
        };

        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        string outDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

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
            cadImage.Dispose();
        }
    }
}