using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class ExportPdfWithCallback
{
    static void Main()
    {
        // Input DXB file path
        string inputPath = "sample.dxb";

        // Verify that the input file exists before attempting to load
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Load options – ignore errors but capture them for inspection
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage;
        try
        {
            // Load the DXB drawing
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load the drawing: {ex.Message}");
            // Report any errors collected during loading
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {err.Message}");
            }
            return;
        }

        // Configure rasterization options for vector export
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1600,
            UnitType = UnitType.Centimenter,
            LineScale = 0.5f,
            // Export callback to indicate success or failure
            RenderResult = result =>
            {
                if (result.IsRenderComplete)
                {
                    Console.WriteLine("PDF export succeeded.");
                }
                else
                {
                    Console.WriteLine("PDF export failed.");
                    if (result.Failures != null)
                    {
                        foreach (var failure in result.Failures)
                        {
                            Console.WriteLine($"Failure: {failure.Message}");
                        }
                    }
                }
            }
        };

        // PDF export options using the rasterization settings above
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Output PDF file path
        string outputPath = "output.pdf";

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Save the drawing as PDF; the callback defined above will be invoked
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during PDF export: {ex.Message}");
        }
    }
}