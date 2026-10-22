using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

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

        // Load the CAD file with error ignoring enabled
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            foreach (var err in loadOptions.Errors)
                Console.WriteLine($"Load error: {err.Message}");
            return;
        }

        // Configure rasterization options and attach a render callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 0.5f,
            PageWidth = 800,
            PageHeight = 600,
            AutomaticLayoutsScaling = true,
            Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
        };

        // Callback to capture rendering errors during export
        rasterOptions.RenderResult = result =>
        {
            if (!result.IsRenderComplete)
            {
                Console.WriteLine("Rendering incomplete.");
                foreach (var failure in result.Failures)
                {
                    Console.WriteLine($"Render error: {failure.Message}");
                }
            }
        };

        // Set up PDF export options and assign the rasterization settings
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Export the CAD drawing to PDF
        try
        {
            cadImage.Save(outputPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save PDF: {ex.Message}");
            return;
        }

        // Report any load-time warnings after successful processing
        if (loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load completed with warnings:");
            foreach (var err in loadOptions.Errors)
                Console.WriteLine($"Load warning: {err.Message}");
        }

        Console.WriteLine("PDF export completed successfully.");
    }
}