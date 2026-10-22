using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class FbxToPdfExporter
{
    // List to collect error messages from the export process
    private static readonly List<string> _exportErrors = new List<string>();

    static void Main()
    {
        string inputPath = @"C:\Temp\model.fbx";
        string outputPath = @"C:\Temp\model.pdf";

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

        // Load the FBX file with error‑ignoring options
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
            // Capture load exception message
            _exportErrors.Add($"Load error: {ex.Message}");
            Console.WriteLine("Failed to load the FBX file.");
            ReportErrors();
            return;
        }

        // Configure rasterization options and attach the render‑result callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            LineScale = 0.01f,
            // Optional: set margins if needed
            //Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
        };

        // Callback invoked after rendering; captures any failures
        rasterOptions.RenderResult = (result) =>
        {
            if (!result.IsRenderComplete)
            {
                foreach (var failure in result.Failures)
                {
                    // Each failure contains a Message describing the problem
                    _exportErrors.Add($"Render failure: {failure.Message}");
                }
            }
        };

        // Set up PDF export options
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Attempt to save as PDF
        try
        {
            cadImage.Save(outputPath, pdfOptions);
        }
        catch (Exception ex)
        {
            // Capture export exception message
            _exportErrors.Add($"Export exception: {ex.Message}");
        }
        finally
        {
            // Dispose the image if it implements IDisposable
            (cadImage as IDisposable)?.Dispose();
        }

        // Report any captured errors
        ReportErrors();
    }

    private static void ReportErrors()
    {
        if (_exportErrors.Count == 0)
        {
            Console.WriteLine("Export completed without errors.");
            return;
        }

        Console.WriteLine("Errors captured during export:");
        foreach (var msg in _exportErrors)
        {
            Console.WriteLine(msg);
        }
    }
}