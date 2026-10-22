using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class ExportFbxToPdfWithErrorHandling
{
    static void Main()
    {
        // Input and output file paths
        string inputPath = "sample.fbx";
        string outputPath = "output.pdf";

        // Verify that the input file exists before attempting to load it
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Configure load options to ignore errors and collect them
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        try
        {
            // Load the FBX file as a CadImage
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                // Set up rasterization options for PDF export
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter, // SDK spelling
                    LineScale = 0.5f
                };

                // Register a callback to capture export failures
                rasterOptions.RenderResult = result =>
                {
                    // 'result' is a CadRenderResult containing a collection of failures
                    if (result?.Failures != null && result.Failures.Count > 0)
                    {
                        foreach (var failure in result.Failures)
                        {
                            // Each failure is a RenderResult with Message and RenderCode
                            string message = failure.Message;
                            var code = failure.RenderCode;

                            // Record the stack trace at the point of failure
                            string stackTrace = Environment.StackTrace;

                            // Log the failure details
                            Console.WriteLine($"Export failure: {message}, Code: {code}");
                            Console.WriteLine($"StackTrace: {stackTrace}");
                        }
                    }
                };

                // Configure PDF export options and assign rasterization settings
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed successfully: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions thrown during load or export
            Console.WriteLine($"Exception during processing: {ex.Message}");

            // Inspect collected load errors, if any
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                foreach (var loadError in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {loadError.Message}, Code: {loadError.RenderCode}");
                }
            }
        }
    }
}