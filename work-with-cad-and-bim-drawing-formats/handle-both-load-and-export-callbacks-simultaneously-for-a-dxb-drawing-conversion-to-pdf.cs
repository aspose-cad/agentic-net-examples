using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class DxbToPdfConverter
{
    static void Main()
    {
        // Input and output paths
        string inputPath = @"input.dxb";
        string outputPath = @"output.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options with error ignoring
        LoadOptions loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;

        // Load the DXB drawing
        try
        {
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during load: {ex.Message}");
        }

        // Report any load errors collected in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Any())
        {
            foreach (var err in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {err.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.WriteLine("Failed to load CAD image.");
            return;
        }

        // Configure rasterization options with callbacks
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 1.0f,
            RenderResult = renderResult =>
            {
                // Check if rendering completed successfully
                if (!renderResult.IsRenderComplete)
                {
                    Console.WriteLine("Render did not complete.");
                }

                // Report any rendering failures
                if (renderResult.Failures != null && renderResult.Failures.Any())
                {
                    foreach (var failure in renderResult.Failures)
                    {
                        Console.WriteLine($"Render failure: {failure.Message}");
                    }
                }
            }
        };

        // Set up PDF export options using the rasterization options
        PdfOptions pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the CAD image as PDF
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"Successfully saved PDF to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during save: {ex.Message}");
        }
    }
}