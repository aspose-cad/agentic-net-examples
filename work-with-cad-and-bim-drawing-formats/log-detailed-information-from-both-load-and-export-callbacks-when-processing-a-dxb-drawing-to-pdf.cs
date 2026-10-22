using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class DxbToPdfConverter
{
    static void Main()
    {
        // Input DXB file and output PDF file paths
        string inputPath = "sample.dxb";
        string outputPath = "output.pdf";

        // Verify input file exists
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

        // Prepare load options – continue loading even if errors occur
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage = null;

        // Load the DXB drawing with the specified load options
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception during loading: {ex.Message}");
        }

        // Log detailed load errors (if any)
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.Error.WriteLine("Load errors:");
            foreach (var error in loadOptions.Errors)
            {
                Console.Error.WriteLine($"{error.RenderCode}: {error.Message}");
            }
        }

        // Proceed only if the image was loaded successfully
        if (cadImage != null)
        {
            // Configure rasterization options with a render-result callback
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1600,
                RenderResult = result =>
                {
                    Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                    if (result.Failures != null && result.Failures.Count > 0)
                    {
                        Console.Error.WriteLine("Render failures:");
                        foreach (var failure in result.Failures)
                        {
                            Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                        }
                    }
                }
            };

            // Set PDF export options using the rasterization settings
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the drawing to PDF, handling any exceptions
            try
            {
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully saved PDF to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception during saving: {ex.Message}");
            }

            // Release resources
            cadImage.Dispose();
        }
        else
        {
            Console.Error.WriteLine("Failed to load the DXB drawing; export aborted.");
        }
    }
}