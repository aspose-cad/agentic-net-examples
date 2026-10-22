using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input CF2 file and desired PDF output
        string inputPath = "sample.cf2";
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
        {
            Directory.CreateDirectory(outDir);
        }

        // LoadOptions – capture warnings without throwing
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
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            return;
        }

        // Report any load‑time warnings collected in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load completed with warnings:");
            foreach (var warning in loadOptions.Errors)
            {
                Console.WriteLine($"- {warning.Message}");
            }
        }

        // Configure rasterization options for PDF export
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            PageWidth = 800,
            PageHeight = 600,
            LineScale = 1.0f,
            // Export callback – invoked after each render pass
            RenderResult = result =>
            {
                if (!result.IsRenderComplete)
                {
                    Console.WriteLine("Render incomplete.");
                    if (result.Failures != null && result.Failures.Count > 0)
                    {
                        foreach (var fail in result.Failures)
                        {
                            Console.WriteLine($"  Failure: {fail.Message}");
                        }
                    }
                }
            }
        };

        // PDF export options, linking rasterization settings
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Perform conversion and handle export errors
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"Conversion succeeded. PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during PDF export: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}