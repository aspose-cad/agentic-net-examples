using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadToPdfExportWithCallbacks
{
    class Program
    {
        // Entry point of the console application
        static void Main(string[] args)
        {
            // -----------------------------------------------------------------
            // Configuration: input CAD file, output directory and PDF file name
            // -----------------------------------------------------------------
            string inputFilePath = "sample.cf2";                     // source CAD file (CF2 format)
            string outputDirectory = "Output";                       // folder for generated files
            string outputFileName = "sample_converted.pdf";          // resulting PDF file name
            string outputFilePath = Path.Combine(outputDirectory, outputFileName);

            // -----------------------------------------------------------------
            // Validate input file existence
            // -----------------------------------------------------------------
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Error: Input file '{inputFilePath}' does not exist.");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory '{outputDirectory}'. {ex.Message}");
                    return;
                }
            }

            // -----------------------------------------------------------------
            // Prepare collections to capture load‑time and render‑time errors
            // -----------------------------------------------------------------
            List<string> loadErrors = new List<string>();
            List<string> renderErrors = new List<string>();

            // -----------------------------------------------------------------
            // Load the CAD file with LoadOptions that collect errors
            // -----------------------------------------------------------------
            LoadOptions loadOptions = new LoadOptions
            {
                // Instruct the loader to continue processing even if errors are encountered
                IgnoreErrors = true
            };

            try
            {
                using (Image cadImage = Image.Load(inputFilePath, loadOptions))
                {
                    // Cast to CadImage to access CAD‑specific properties
                    Image cad = cadImage ;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Error: Loaded image is not a CAD image.");
                        return;
                    }

                    // -----------------------------------------------------------------
                    // Capture any errors reported during the loading phase
                    // -----------------------------------------------------------------
                    if (loadOptions.Errors != null)
                    {
                        foreach (var err in loadOptions.Errors)
                        {
                            // RenderResult contains Message and RenderCode
                            loadErrors.Add($"Load error: {err.Message} (Code: {err.RenderCode})");
                        }
                    }

                    // -----------------------------------------------------------------
                    // Configure PDF export options with rasterization settings
                    // -----------------------------------------------------------------
                    PdfOptions pdfOptions = new PdfOptions();

                    // VectorRasterizationOptions controls how vector data is rasterized
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Example: set page size to A4 (210mm x 297mm) in points (1 point = 1/72 inch)
                        PageWidth = 595,   // 8.27 inches * 72
                        PageHeight = 842,  // 11.69 inches * 72
                        // Preserve layers; you can filter by name if required
                        Layers = null,
                        // No specific layout filtering
                        Layouts = null,
                        // Set a reasonable line scale for visibility
                        LineScale = 1.0f
                    };

                    // -----------------------------------------------------------------
                    // Assign a render result callback to capture rendering failures
                    // -----------------------------------------------------------------
                    rasterOptions.RenderResult = result =>
                    {
                        // If rendering did not complete successfully, record each failure
                        if (!result.IsRenderComplete && result.Failures != null)
                        {
                            foreach (var failure in result.Failures)
                            {
                                renderErrors.Add($"Render failure: {failure.Message} (Code: {failure.RenderCode})");
                            }
                        }
                    };

                    // Attach rasterization options to the PDF options
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // -----------------------------------------------------------------
                    // Perform the conversion and save the PDF file
                    // -----------------------------------------------------------------
                    try
                    {
                        cad.Save(outputFilePath, pdfOptions);
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Error during PDF save: {saveEx.Message}");
                        return;
                    }

                    // -----------------------------------------------------------------
                    // Report any load‑time errors collected earlier
                    // -----------------------------------------------------------------
                    if (loadErrors.Count > 0)
                    {
                        Console.WriteLine("Load‑time errors detected:");
                        foreach (var msg in loadErrors)
                        {
                            Console.WriteLine(msg);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No load‑time errors detected.");
                    }

                    // -----------------------------------------------------------------
                    // Report any render‑time errors captured by the callback
                    // -----------------------------------------------------------------
                    if (renderErrors.Count > 0)
                    {
                        Console.WriteLine("Render‑time errors detected:");
                        foreach (var msg in renderErrors)
                        {
                            Console.WriteLine(msg);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Render completed without errors.");
                    }

                    Console.WriteLine($"PDF successfully saved to: {outputFilePath}");
                } // using cadImage
            }
            catch (Exception ex)
            {
                // General exception handling for unexpected failures
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}