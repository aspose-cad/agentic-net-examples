using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Define input and output paths
        string inputFile = "input.fbx";
        string outputDir = "output";
        string outputFile = Path.Combine(outputDir, "result.pdf");

        try
        {
            // Verify source file exists
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Source file not found: {inputFile}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare load options – ignore errors to allow processing and collect them later
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the FBX drawing
            Image image = Image.Load(inputFile, loadOptions);

            // Report any load‑time render results
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                foreach (RenderResult loadResult in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"Load warning: {loadResult.Message} (Code: {loadResult.RenderCode})");
                }
            }

            // Configure rasterization options with a callback to capture rendering exceptions
            var rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter,
                PageWidth = 210,   // A4 width in mm (converted later by Aspose)
                PageHeight = 297,  // A4 height in mm
                RenderResult = renderResult =>
                {
                    // The exact properties of CadRenderResult may vary between SDK versions.
                    // Log a generic message to capture that a rendering issue occurred.
                    Console.Error.WriteLine("Render issue occurred during PDF export.");
                }
            };

            // Prepare PDF export options and attach rasterization settings
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the drawing as PDF
            image.Save(outputFile, pdfOptions);

            Console.WriteLine($"Export completed successfully: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}