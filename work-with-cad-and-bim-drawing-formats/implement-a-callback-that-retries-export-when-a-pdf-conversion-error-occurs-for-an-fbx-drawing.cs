using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

public class FbxPdfExporter
{
    // Exports an FBX drawing to PDF with retry logic for PDF conversion failures.
    public static void ExportFbxToPdf(string inputFilePath, string outputPdfPath)
    {
        // Validate input file.
        if (!File.Exists(inputFilePath))
            throw new FileNotFoundException("Input FBX file not found.", inputFilePath);

        // Ensure output directory exists.
        string outputDir = Path.GetDirectoryName(outputPdfPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        // Prepare load options – ignore errors to allow processing and later inspection.
        var loadOptions = new LoadOptions { IgnoreErrors = true };

        CadImage cadImage;
        try
        {
            cadImage = (CadImage)Image.Load(inputFilePath, loadOptions);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load FBX file '{inputFilePath}'.", ex);
        }

        // Capture render failures from the callback.
        List<RenderResult> lastRenderFailures = new List<RenderResult>();

        // Configure rasterization options with a RenderResult callback.
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1200,
            RenderResult = result =>
            {
                // Store failures for later analysis.
                lastRenderFailures = result.Failures?.ToList() ?? new List<RenderResult>();

                // Log each failure.
                foreach (var failure in lastRenderFailures)
                {
                    Console.Error.WriteLine($"Render failure - Code: {failure.RenderCode}, Message: {failure.Message}");
                }
            }
        };

        // Configure PDF export options.
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        const int maxRetries = 3;
        int attempt = 0;
        bool exportSucceeded = false;

        while (attempt < maxRetries && !exportSucceeded)
        {
            attempt++;

            try
            {
                // Attempt to save the CAD image as PDF.
                cadImage.Save(outputPdfPath, pdfOptions);
                exportSucceeded = true; // Save succeeded without throwing.
            }
            catch (Exception ex)
            {
                // Determine if the failure is due to PDF conversion.
                bool pdfConversionError = lastRenderFailures
                    .Any(f => f.RenderCode == RenderErrorCode.PdfRendererFailed);

                if (pdfConversionError && attempt < maxRetries)
                {
                    Console.Error.WriteLine($"Attempt {attempt} failed due to PDF conversion error. Retrying...");
                    // Optionally adjust raster options before retry.
                }
                else
                {
                    throw new InvalidOperationException(
                        $"PDF export failed on attempt {attempt}.", ex);
                }
            }
        }

        // Clean up.
        cadImage.Dispose();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Example usage with placeholder paths.
        string inputPath = "path/to/input.fbx";   // Replace with actual FBX file path.
        string outputPath = "path/to/output.pdf"; // Replace with desired PDF output path.

        try
        {
            FbxPdfExporter.ExportFbxToPdf(inputPath, outputPath);
            Console.WriteLine("Export completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Export failed: {ex.Message}");
        }
    }
}