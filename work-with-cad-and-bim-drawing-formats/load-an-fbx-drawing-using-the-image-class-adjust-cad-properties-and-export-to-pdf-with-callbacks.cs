using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadFbxToPdf
{
    static void Main()
    {
        // Input and output paths
        string inputPath = "input.fbx";
        string outputPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Prepare load options
        var loadOptions = new LoadOptions
        {
            // Continue loading even if non‑critical errors occur
            IgnoreErrors = true
        };

        try
        {
            // Load the FBX drawing as a CadImage
            using (var cadImage = Image.Load(inputPath, loadOptions))
            {
                // Report any load errors collected in LoadOptions.Errors
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Load completed with the following warnings:");
                    foreach (var err in loadOptions.Errors)
                    {
                        Console.WriteLine($"- {err}");
                    }
                }

                // -----------------------------------------------------------------
                // Adjust CAD rasterization properties before export
                // -----------------------------------------------------------------
                var rasterOptions = new CadRasterizationOptions
                {
                    // Unit type spelling follows the SDK enum (Centimenter)
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // Register a render‑completion callback
                rasterOptions.RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.WriteLine("Rendering did not complete successfully.");
                        if (result.Failures != null)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.WriteLine($"Render failure: {failure}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Rendering completed successfully.");
                    }
                };

                // -----------------------------------------------------------------
                // Prepare PDF export options and attach rasterization settings
                // -----------------------------------------------------------------
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Handle any unexpected exceptions during load or save
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}