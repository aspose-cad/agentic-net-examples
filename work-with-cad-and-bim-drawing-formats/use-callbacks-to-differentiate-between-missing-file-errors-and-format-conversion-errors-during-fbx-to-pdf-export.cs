using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions; // for RenderResult and CadRenderResult

class FbxToPdfExport
{
    static void Main()
    {
        // Input FBX file and output PDF file paths
        string inputPath = "sample.fbx";
        string outputPath = "output.pdf";

        // Guard for missing input file
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options – enable ignoring errors to allow processing to continue
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;

        // Attempt to load the FBX file
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (FileNotFoundException fnfEx)
        {
            // Missing file error (should be caught by guard above, but kept for safety)
            Console.Error.WriteLine($"File not found: {fnfEx.Message}");
            return;
        }
        catch (Exception ex)
        {
            // Other loading errors (e.g., unsupported format)
            Console.Error.WriteLine($"Error loading file: {ex.Message}");
            return;
        }

        // Check for load‑time errors reported via LoadOptions.Errors
        if (loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load errors detected:");
            foreach (RenderResult error in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{error.RenderCode}: {error.Message}");
            }
        }

        // Configure rasterization options for PDF export
        var rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600,
            PageHeight = 1600,
            Layouts = new[] { "Model" }
        };

        // Callback to differentiate rendering problems (format conversion errors)
        rasterOptions.RenderResult = result =>
        {
            if (!result.IsRenderComplete)
            {
                Console.Error.WriteLine("Rendering did not complete successfully.");
            }

            foreach (RenderResult failure in result.Failures)
            {
                Console.Error.WriteLine($"Render failure {failure.RenderCode}: {failure.Message}");
            }
        };

        // Set PDF export options using the rasterization settings
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the CAD image as PDF
        try
        {
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine("FBX successfully exported to PDF.");
        }
        catch (Exception ex)
        {
            // Errors occurring during the save operation
            Console.Error.WriteLine($"Error saving PDF: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}