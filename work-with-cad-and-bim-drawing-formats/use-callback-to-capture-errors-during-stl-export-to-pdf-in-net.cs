using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadStlToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input STL file and output PDF paths (replace with actual paths as needed)
            string inputPath = "model.stl";
            string outputPath = "output.pdf";

            // Guard against missing input file
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load options with error collection enabled
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            Image cadImage = null;

            try
            {
                // Load the STL file
                cadImage = Image.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load STL file: {ex.Message}");
                return;
            }

            // Report any non‑fatal load errors
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.Error.WriteLine("Load-time errors/warnings:");
                foreach (RenderResult loadError in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"{loadError.RenderCode}: {loadError.Message}");
                }
            }

            // Configure rasterization options with a render result callback
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 800f,
                PageHeight = 600f,
                RenderResult = result =>
                {
                    if (result.Failures != null && result.Failures.Count > 0)
                    {
                        Console.Error.WriteLine("Render-time errors/warnings:");
                        foreach (RenderResult failure in result.Failures)
                        {
                            Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                        }
                    }

                    Console.WriteLine($"Render complete: {result.IsRenderComplete}");
                }
            };

            // Set PDF export options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            try
            {
                // Save the CAD image as a PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save PDF: {ex.Message}");
            }
            finally
            {
                cadImage?.Dispose();
            }
        }
    }
}