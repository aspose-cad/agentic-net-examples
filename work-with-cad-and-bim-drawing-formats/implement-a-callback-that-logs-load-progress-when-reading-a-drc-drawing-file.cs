using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadLoadProgressDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the DRC drawing file
            string inputFile = "sample.drc";

            // Verify that the input file exists
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Configure load options
            var loadOptions = new LoadOptions();
            loadOptions.IgnoreErrors = true; // Continue loading even if errors occur

            // NOTE: Aspose.CAD does not provide a load‑progress callback for DRC files.
            // The SDK only allows inspection of errors after the load operation completes.

            try
            {
                // Log start of loading
                Console.WriteLine($"Loading DRC file: {inputFile}");

                // Load the drawing
                using (CadImage cadImage = (CadImage)Image.Load(inputFile, loadOptions))
                {
                    // Log successful load
                    Console.WriteLine("DRC file loaded successfully.");

                    // Optionally, inspect any errors collected during loading
                    if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                    {
                        Console.WriteLine("Load completed with the following warnings/errors:");
                        foreach (RenderResult error in loadOptions.Errors)
                        {
                            Console.WriteLine($"- {error.Message} (Code: {error.RenderCode})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log load failure
                Console.WriteLine($"Failed to load DRC file: {ex.Message}");

                // If errors were captured before the exception, list them
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Collected load errors:");
                    foreach (RenderResult error in loadOptions.Errors)
                    {
                        Console.WriteLine($"- {error.Message} (Code: {error.RenderCode})");
                    }
                }
            }
        }
    }
}