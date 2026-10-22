using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class IfcToPngExportPerformance
{
    static void Main()
    {
        // Folder containing IFC files
        string inputFolder = @"C:\InputIfc";
        // Output folder for PNG files
        string outputFolder = @"C:\OutputPng";

        // Validate input folder
        if (!Directory.Exists(inputFolder))
        {
            Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
            return;
        }

        // Ensure output folder exists
        Directory.CreateDirectory(outputFolder);

        // Get all IFC files in the input folder
        string[] ifcFiles = Directory.GetFiles(inputFolder, "*.ifc");

        foreach (string ifcPath in ifcFiles)
        {
            // Guard against missing file (validation requirement)
            if (!File.Exists(ifcPath))
            {
                Console.Error.WriteLine($"File not found: {ifcPath}");
                continue;
            }

            // Prepare load options (ignore errors to continue processing)
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            CadImage cadImage = null;
            try
            {
                cadImage = (CadImage)Image.Load(ifcPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load '{ifcPath}': {ex.Message}");
                continue;
            }

            using (cadImage)
            {
                // Report any load errors
                foreach (var loadError in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"{loadError.RenderCode}: {loadError.Message}");
                }

                // Configure rasterization options for PNG export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Provide layout names (string[])
                    Layouts = cadImage.Layouts.Keys.ToArray(),
                    RenderResult = result =>
                    {
                        // Log any rendering failures
                        foreach (var failure in result.Failures)
                        {
                            Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                        }

                        // Note incomplete renders
                        if (!result.IsRenderComplete)
                        {
                            Console.Error.WriteLine("Render incomplete for current export.");
                        }
                    }
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Measure export duration
                Stopwatch stopwatch = Stopwatch.StartNew();

                try
                {
                    // Build output file path
                    string outputFileName = Path.GetFileNameWithoutExtension(ifcPath) + ".png";
                    string outputPath = Path.Combine(outputFolder, outputFileName);

                    // Perform the export
                    cadImage.Save(outputPath, pngOptions);
                }
                catch (Exception ex)
                {
                    // Report any save errors
                    Console.Error.WriteLine($"Error exporting '{ifcPath}': {ex.Message}");
                }
                finally
                {
                    stopwatch.Stop();
                    // Record duration for performance analysis
                    Console.WriteLine($"Export of '{ifcPath}' completed in {stopwatch.ElapsedMilliseconds} ms.");
                }
            }
        }
    }
}