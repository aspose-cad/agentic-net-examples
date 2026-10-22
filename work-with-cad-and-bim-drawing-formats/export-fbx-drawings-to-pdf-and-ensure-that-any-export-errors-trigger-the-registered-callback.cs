using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input FBX file path
        string inputPath = "input.fbx";
        // Output PDF file path
        string outputPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure load options to continue on errors
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        // Load the CAD image
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

        // Report any load errors collected in LoadOptions
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load completed with errors:");
            foreach (RenderResult err in loadOptions.Errors)
            {
                // RenderResult provides Message and RenderCode in the current API
                Console.WriteLine($"- {err.Message} (Code: {err.RenderCode})");
            }
        }

        // Set up rasterization options with a render result callback
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            PageWidth = 8.5f,
            PageHeight = 11f,
            Layouts = null // use all layouts
        };

        // Register callback to capture rendering results
        rasterOptions.RenderResult = result =>
        {
            // The current SDK does not expose Message or RenderCode on CadRenderResult.
            // We can only acknowledge that a render result was received.
            Console.WriteLine("Render result callback invoked.");
        };

        // Configure PDF export options
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the image as PDF
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine("Export to PDF completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to export PDF: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}