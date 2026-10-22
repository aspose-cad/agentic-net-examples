using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class FbxToPdfExport
{
    static void Main()
    {
        string inputPath = "input.fbx";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        LoadOptions loadOptions = new LoadOptions
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
            Console.Error.WriteLine($"Exception while loading file: {ex.Message}");
            return;
        }

        if (loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load completed with errors:");
            foreach (var err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
        }

        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600f,
            PageHeight = 1200f,
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.Error.WriteLine("Rendering was not completed.");
                }

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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception while saving PDF: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}