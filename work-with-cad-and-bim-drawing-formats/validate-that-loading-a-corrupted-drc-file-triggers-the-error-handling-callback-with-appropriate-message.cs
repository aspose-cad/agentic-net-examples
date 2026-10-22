using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadLoadDiagnostics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the corrupted DRC file
            string inputPath = "corrupted.drc";

            // Guard: ensure the file exists before attempting to load
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare load options to collect errors without throwing
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true // Continue loading and collect errors
            };

            CadImage cadImage = null;
            try
            {
                // Attempt to load the corrupted file
                cadImage = (CadImage)Image.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                // If an exception is thrown, output it
                Console.WriteLine("Exception during loading: " + ex.Message);
            }

            // After loading attempt, inspect collected errors
            List<RenderResult> errors = loadOptions.Errors;
            if (errors != null && errors.Count > 0)
            {
                Console.WriteLine("Load errors detected:");
                foreach (RenderResult error in errors)
                {
                    Console.WriteLine($"- Message: {error.Message}");
                    Console.WriteLine($"  Code   : {error.RenderCode}");
                }
            }
            else
            {
                Console.WriteLine("No load errors were reported.");
            }

            // Dispose the image if it was successfully created
            cadImage?.Dispose();
        }
    }
}