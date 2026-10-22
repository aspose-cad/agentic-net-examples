using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadExportErrorHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file (DWG, DXF, etc.) and output DWF file paths.
            string inputPath = "sample.dwg";
            string outputPath = "output.dwf";

            // Guard against missing input file.
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // -----------------------------------------------------------------
            // 1. Prepare load options.
            //    - IgnoreErrors = false forces the loader to report errors but not to abort.
            //    - Errors collection will contain any rendering issues detected during load.
            // -----------------------------------------------------------------
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = false
            };

            // -----------------------------------------------------------------
            // 2. Load the CAD image inside a try/catch block.
            // -----------------------------------------------------------------
            try
            {
                using (Image image = Image.Load(inputPath, loadOptions))
                {
                    // Report load‑time errors, if any.
                    if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                    {
                        Console.WriteLine("Load completed with the following errors:");
                        foreach (RenderResult err in loadOptions.Errors)
                        {
                            Console.WriteLine($"  Code: {err.RenderCode}, Message: {err.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Load completed without errors.");
                    }

                    // -----------------------------------------------------------------
                    // 3. Configure rasterization options for the export.
                    //    - Set a RenderResult callback to capture rendering diagnostics.
                    // -----------------------------------------------------------------
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        LineScale = 1.0f,
                        Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High,
                            Arc = RasterizationQualityValue.High
                        },
                        RenderResult = (CadRenderResult renderResult) =>
                        {
                            Console.WriteLine("RenderResult callback invoked.");

                            if (renderResult.IsRenderComplete)
                            {
                                Console.WriteLine("  Rendering reported as complete.");
                            }
                            else
                            {
                                Console.WriteLine("  Rendering reported as incomplete.");
                            }

                            if (renderResult.Failures != null && renderResult.Failures.Count > 0)
                            {
                                Console.WriteLine("  Rendering failures:");
                                foreach (RenderResult failure in renderResult.Failures)
                                {
                                    Console.WriteLine($"    Code: {failure.RenderCode}, Message: {failure.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("  No rendering failures reported.");
                            }
                        }
                    };

                    // -----------------------------------------------------------------
                    // 4. Prepare DWF export options and assign the rasterization options.
                    // -----------------------------------------------------------------
                    var dwfOptions = new DwfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // -----------------------------------------------------------------
                    // 5. Save the image to DWF inside a try/catch block.
                    // -----------------------------------------------------------------
                    try
                    {
                        image.Save(outputPath, dwfOptions);
                        Console.WriteLine($"Image successfully saved to '{outputPath}'.");
                    }
                    catch (Exception saveEx)
                    {
                        Console.WriteLine("An error occurred during saving:");
                        Console.WriteLine(saveEx.Message);
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.WriteLine("An error occurred during loading:");
                Console.WriteLine(loadEx.Message);
            }
        }
    }
}