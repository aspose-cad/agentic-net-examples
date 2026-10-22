using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class DwfExportWithErrorCallback
{
    static void Main()
    {
        try
        {
            // Input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists before attempting to load
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

            // Load the CAD image using the configured load options
            CadImage cadImage = (CadImage)Image.Load(inputPath, loadOptions);

            // Inspect any errors that occurred during loading
            foreach (var loadError in loadOptions.Errors)
            {
                Console.WriteLine($"Load error: {loadError.Message}");
            }

            // NOTE: The current Aspose.CAD SDK does not provide a DWF export option
            // with a render-result callback. Therefore, handling rendering errors
            // via a callback during DWF export is not supported.
            Console.WriteLine("DWF export with error callback is not supported by the current SDK.");

            // Clean up
            cadImage.Dispose();

            Console.WriteLine("Processing completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}