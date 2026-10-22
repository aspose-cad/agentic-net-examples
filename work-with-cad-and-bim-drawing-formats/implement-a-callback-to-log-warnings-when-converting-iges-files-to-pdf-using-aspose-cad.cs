using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class IgesToPdfConverter
{
    static void Main()
    {
        // Input IGES file and output PDF paths
        string sourcePath = "sample.iges";
        string outputPath = "output.pdf";

        // Verify that the source file exists before attempting to load it
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Configure load options to ignore errors and collect them in the Errors list
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;
        try
        {
            // Load the IGES file with the specified load options
            cadImage = Image.Load(sourcePath, loadOptions);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during loading
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            return;
        }

        // Log any warnings that were captured during the load process
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            foreach (var loadError in loadOptions.Errors)
            {
                Console.WriteLine($"Load warning: {loadError.Message} (Code: {loadError.RenderCode})");
            }
        }

        // Set up rasterization options for PDF conversion
        var rasterOptions = new CadRasterizationOptions
        {
            // Example page size; adjust as needed
            PageWidth = 1600,
            PageHeight = 1600,

            // Callback invoked after each rendering operation
            RenderResult = renderResult =>
            {
                // Log any rendering warnings or failures
                if (renderResult.Failures != null && renderResult.Failures.Count > 0)
                {
                    foreach (var failure in renderResult.Failures)
                    {
                        Console.WriteLine($"Render warning: {failure.Message} (Code: {failure.RenderCode})");
                    }
                }
            }
        };

        // Configure PDF export options using the rasterization settings above
        var pdfOptions = new PdfOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the CAD image as PDF, ensuring proper disposal of the CadImage object
        using (cadImage)
        {
            cadImage.Save(outputPath, pdfOptions);
        }

        Console.WriteLine($"Conversion completed. PDF saved to: {outputPath}");
    }
}