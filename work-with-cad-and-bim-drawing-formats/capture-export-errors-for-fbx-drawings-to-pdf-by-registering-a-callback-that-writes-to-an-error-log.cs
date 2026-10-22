using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Paths for input FBX, output PDF and error log
        string inputPath = "sample.fbx";
        string outputPath = "output.pdf";
        string logPath = "export_errors.log";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load options – ignore errors to allow processing to continue
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Open the log file before the export so the callback can write to it
            using (var logWriter = new StreamWriter(logPath, true))
            {
                // Load the FBX drawing as a CadImage
                using (var cadImage = Image.Load(inputPath, loadOptions))
                {
                    // Configure rasterization options and register the render result callback
                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f
                    };

                    // Callback invoked after rendering; capture any failures
                    rasterOptions.RenderResult = result =>
                    {
                        if (!result.IsRenderComplete)
                        {
                            foreach (var failure in result.Failures)
                            {
                                // failure.Message and failure.RenderCode are available on RenderResult
                                logWriter.WriteLine($"{DateTime.Now}: {failure.Message} (Code: {failure.RenderCode})");
                            }
                        }
                    };

                    // Set up PDF export options and assign the rasterization options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Export the drawing to PDF; the callback will log any errors encountered
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}