using System;
using System.IO;
using System.Threading;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        // Input and output file paths
        string inputPath = "sample.dwg";
        string pngOutputPath = "output.png";
        string dxfOutputPath = "output.dxf";

        // Ensure input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directories exist
        CreateOutputDirectory(pngOutputPath);
        CreateOutputDirectory(dxfOutputPath);

        // Configure load options
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage cadImage = null;
        try
        {
            // Load the CAD file with the specified options
            cadImage = (CadImage)Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load CAD file: " + ex.Message);
            foreach (var error in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {error.Message}, Code: {error.RenderCode}");
            }
            return;
        }

        // Report non‑fatal load warnings, if any
        if (loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Non‑fatal load warnings:");
            foreach (var warning in loadOptions.Errors)
            {
                Console.WriteLine($"Warning: {warning.Message}, Code: {warning.RenderCode}");
            }
        }

        // Set up PNG rasterization options with a render result callback
        var pngOptions = new PngOptions();
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            UnitType = UnitType.Centimenter,
            Layouts = new[] { "Model" },
            RenderResult = result =>
            {
                if (result.IsRenderComplete)
                {
                    Console.WriteLine("Render completed successfully.");
                }
                else
                {
                    foreach (var failure in result.Failures)
                    {
                        Console.WriteLine($"Render issue: {failure.Message}, Code: {failure.RenderCode}");
                    }
                }
            }
        };
        pngOptions.VectorRasterizationOptions = rasterOptions;

        // Save the CAD image as PNG with retry logic
        SaveWithRetry(() => cadImage.Save(pngOutputPath, pngOptions), "PNG export");

        // Set up DXF options
        var dxfOptions = new DxfOptions
        {
            OutputMode = CadOutputMode.Convert,
            DxfFileFormat = CadFileFormat.Binary,
            Version = DxfOutputVersion.R12
        };

        // Save the CAD image as DXF with retry logic
        SaveWithRetry(() => cadImage.Save(dxfOutputPath, dxfOptions), "DXF export");

        // Clean up
        cadImage.Dispose();

        Console.WriteLine("Processing completed.");
    }

    private static void CreateOutputDirectory(string filePath)
    {
        string outputDir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
    }

    private static void SaveWithRetry(Action saveAction, string description, int maxAttempts = 3, int delayMs = 1000)
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                attempt++;
                saveAction();
                Console.WriteLine($"{description} succeeded on attempt {attempt}.");
                break;
            }
            catch (IOException ioEx) when (attempt < maxAttempts)
            {
                Console.WriteLine($"{description} failed due to I/O error (attempt {attempt}): {ioEx.Message}");
                Console.WriteLine($"Retrying in {delayMs} ms...");
                Thread.Sleep(delayMs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{description} failed: {ex.Message}");
                break;
            }
        }
    }
}