using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input IGES file and output PDF file paths
        string sourcePath = "sample.igs";
        string outputPath = "output.pdf";

        // Guard for source file existence
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – ignore errors to allow loading even if the file has issues
        LoadOptions loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            // Load the IGES file as a CadImage
            cadImage = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception while loading image: {ex.Message}");
        }

        // Report any load errors collected in LoadOptions.Errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load errors:");
            foreach (RenderResult err in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{err.RenderCode}: {err.Message}");
            }
        }

        if (cadImage == null)
        {
            Console.Error.WriteLine("Failed to load CAD image. Exiting.");
            return;
        }

        // Configure rasterization options for PDF export
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1200,
            PageHeight = 800,
            RenderResult = result =>
            {
                // Overall render status
                Console.WriteLine($"Render complete: {result.IsRenderComplete}");

                // Individual failures, if any
                if (result.Failures != null && result.Failures.Count > 0)
                {
                    Console.Error.WriteLine("Render failures:");
                    foreach (RenderResult failure in result.Failures)
                    {
                        Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                    }
                }
            }
        };

        // Set PDF options using the rasterization settings
        PdfOptions pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the CAD image to PDF with error handling
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"PDF successfully saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception while saving PDF: {ex.Message}");
        }
        finally
        {
            cadImage.Dispose();
        }
    }
}