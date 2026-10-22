using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = "input.dxb";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Prepare load options to capture loading errors
        LoadOptions loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        try
        {
            Image cadImage = Image.Load(inputPath, loadOptions);

            // Report any errors collected during loading
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.Error.WriteLine("Loading completed with errors:");
                foreach (RenderResult loadError in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"- {loadError.Message} (Code: {loadError.RenderCode})");
                }
            }

            // Configure rasterization options with an export callback
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                RenderResult = (CadRenderResult renderResult) =>
                {
                    // Check if rendering completed successfully
                    if (!renderResult.IsRenderComplete)
                    {
                        Console.Error.WriteLine("Rendering did not complete successfully.");
                    }

                    // Report any rendering failures
                    if (renderResult.Failures != null && renderResult.Failures.Count > 0)
                    {
                        Console.Error.WriteLine("Rendering completed with errors:");
                        foreach (RenderResult failure in renderResult.Failures)
                        {
                            Console.Error.WriteLine($"- {failure.Message} (Code: {failure.RenderCode})");
                        }
                    }
                }
            };

            // Set PDF save options and attach rasterization options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the CAD drawing as PDF
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An exception occurred: {ex.Message}");
        }
    }
}