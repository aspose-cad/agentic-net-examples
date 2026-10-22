using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadRetryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the STEP file
            string inputPath = "sample.stp";
            // Output PDF path
            string outputPath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            const int maxRetries = 3;
            int attempt = 0;
            bool loaded = false;
            Image cadImage = null;
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            while (attempt < maxRetries && !loaded)
            {
                attempt++;
                try
                {
                    // Attempt to load the STEP file
                    cadImage = Image.Load(inputPath, loadOptions);
                    loaded = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");

                    // Inspect collected load errors, if any
                    if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                    {
                        foreach (RenderResult error in loadOptions.Errors)
                        {
                            Console.WriteLine($"Load error: {error.Message} (Code: {error.RenderCode})");
                        }
                    }

                    if (attempt >= maxRetries)
                    {
                        Console.WriteLine("Maximum retry attempts reached. Unable to load the file.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Retrying load operation...");
                    }
                }
            }

            if (loaded && cadImage != null)
            {
                // Ensure the image is disposed after use
                using (cadImage)
                {
                    // Set up rasterization options for PDF export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1000,
                        PageHeight = 1000,
                        UnitType = UnitType.Point,
                        AutomaticLayoutsScaling = true
                    };

                    // Configure PDF save options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    try
                    {
                        // Save the CAD drawing as PDF
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"File successfully saved to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to save PDF: {ex.Message}");
                    }
                }
            }
        }
    }
}