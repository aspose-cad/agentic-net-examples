using Aspose.CAD.WatermarkGuard;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadIfcExport
{
    class Program
    {
        static void Main()
        {
            // List of IFC files to process
            List<string> ifcFiles = new List<string>
            {
                @"Input\Drawing1.ifc",
                @"Input\Drawing2.ifc",
                // add more file paths as needed
            };

            // Output directory for PNG files
            string outputDir = @"Output\PNGs";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Process each IFC file
            foreach (string inputPath in ifcFiles)
            {
                // Guard against missing input file
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    continue;
                }

                // Load options with error ignoring enabled
                var loadOptions = new Aspose.CAD.LoadOptions
                {
                    IgnoreErrors = true
                };

                CadImage cadImage = null;
                try
                {
                    // Load the IFC drawing
                    cadImage = (CadImage)Image.Load(inputPath, loadOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load '{inputPath}': {ex.Message}");
                    continue;
                }

                // Inspect any load errors collected in LoadOptions
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine($"Load warnings for '{inputPath}':");
                    foreach (var err in loadOptions.Errors)
                    {
                        Console.WriteLine($"  - {err.Message}");
                    }
                }

                // Embed a blind watermark (text) into the CAD image before export
                cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                // Prepare rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    // Set desired unit and line scale
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    // Optional: set margins if needed
                    //Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 },
                };

                // Register render result callback for error handling
                rasterOptions.RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.WriteLine($"Render incomplete for '{inputPath}'.");
                        if (result.Failures != null)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.WriteLine($"  Failure: {failure.Message}");
                            }
                        }
                    }
                };

                // Prepare PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Determine output file path
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputPath);
                string outputPath = Path.Combine(outputDir, $"{fileNameWithoutExt}.png");

                // Save the PNG image
                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Saved PNG: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save PNG for '{inputPath}': {ex.Message}");
                    continue;
                }

                // Validate the blind watermark by reloading the original CAD file
                // (validation must be performed on a CAD image, not on PNG)
                CadImage validationImage = null;
                try
                {
                    validationImage = (CadImage)Image.Load(inputPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to reload CAD for validation: {ex.Message}");
                    continue;
                }

                // Set the expected watermark text for validation
                validationImage.WatermarkGuardService.TextToValidate = "Confidential";

                // Perform validation
                var validationStatus = validationImage.WatermarkGuardService.ValidationStatus();

                // Report validation result
                switch (validationStatus)
                {
                    case WatermarkingStatus.SUCCESS:
                        Console.WriteLine($"Watermark validation SUCCESS for '{inputPath}'.");
                        break;
                    case WatermarkingStatus.FAILURE:
                        Console.WriteLine($"Watermark validation FAILURE for '{inputPath}'.");
                        break;
                    case WatermarkingStatus.UNKNOWN:
                        Console.WriteLine($"Watermark validation UNKNOWN for '{inputPath}'.");
                        break;
                    case WatermarkingStatus.PENDING:
                        Console.WriteLine($"Watermark validation PENDING for '{inputPath}'.");
                        break;
                    default:
                        Console.WriteLine($"Watermark validation returned unexpected status for '{inputPath}'.");
                        break;
                }

                // Cleanup
                cadImage.Dispose();
                validationImage.Dispose();
            }
        }
    }
}