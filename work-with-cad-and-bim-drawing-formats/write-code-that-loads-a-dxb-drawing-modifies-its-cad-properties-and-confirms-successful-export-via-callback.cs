using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dxb";
        string outputPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            Directory.CreateDirectory(outDir);

        // Load options (do not ignore errors)
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = false
        };

        CadImage cadImage = null;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD image: {ex.Message}");
            return;
        }

        // Report any load errors
        if (loadOptions.Errors != null && loadOptions.Errors.Any())
        {
            Console.WriteLine("Load errors:");
            foreach (var err in loadOptions.Errors)
                Console.WriteLine(err);
        }

        // Modify CAD properties
        cadImage.DefaultFont = "Arial";

        // Prepare rasterization options for export
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 0.5f
        };

        // Set render callback to confirm successful export
        rasterOptions.RenderResult = result =>
        {
            if (result.IsRenderComplete && (result.Failures == null || !result.Failures.Any()))
            {
                Console.WriteLine("Render completed successfully.");
            }
            else
            {
                Console.WriteLine("Render completed with failures:");
                if (result.Failures != null)
                {
                    foreach (var failure in result.Failures)
                        Console.WriteLine($" - {failure}");
                }
            }
        };

        // Export to PDF using PdfOptions
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
            Console.WriteLine($"Failed to save CAD image: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}