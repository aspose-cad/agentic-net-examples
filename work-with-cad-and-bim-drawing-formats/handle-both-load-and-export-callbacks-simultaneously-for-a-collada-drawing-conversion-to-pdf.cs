using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class ColladaToPdf
{
    static void Main()
    {
        const string inputPath = "input.dae";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file \"{inputPath}\" does not exist. Aborting.");
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
        }

        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            foreach (var loadError in loadOptions.Errors)
            {
                Console.Error.WriteLine($"Load error {loadError.RenderCode}: {loadError.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.Error.WriteLine("Image could not be loaded; aborting conversion.");
            return;
        }

        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200
        };

        rasterOptions.RenderResult = result =>
        {
            if (!result.IsRenderComplete)
            {
                Console.Error.WriteLine("Rendering did not complete successfully.");
            }

            if (result.Failures != null)
            {
                foreach (var failure in result.Failures)
                {
                    Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
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
            Console.WriteLine($"Conversion completed successfully. PDF saved to \"{outputPath}\".");
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