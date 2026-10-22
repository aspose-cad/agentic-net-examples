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
            // Path to the COLLADA (.dae) file to be loaded
            string inputPath = "sample.dae";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                string placeholderContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                            "<COLLADA xmlns=\"http://www.collada.org/2005/11/COLLADASchema\" version=\"1.4.1\">\n" +
                                            "</COLLADA>";
                try
                {
                    File.WriteAllText(inputPath, placeholderContent);
                    Console.WriteLine("Placeholder COLLADA file created at: " + Path.GetFullPath(inputPath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Configure load options to ignore errors during loading
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the CAD drawing with the specified options
                using (Image cadImage = Image.Load(inputPath, loadOptions))
                {
                    // The drawing was loaded (potentially with errors)
                    Console.WriteLine("CAD drawing loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                // An exception may be thrown for critical failures
                Console.WriteLine($"Exception while loading CAD drawing: {ex.Message}");
            }

            // After loading, inspect the collected errors (if any)
            IList<RenderResult> errors = loadOptions.Errors;
            if (errors != null && errors.Count > 0)
            {
                Console.WriteLine("Load diagnostics - errors captured:");
                foreach (RenderResult error in errors)
                {
                    Console.WriteLine($"- Message: {error.Message}, Code: {error.RenderCode}");
                }
            }
            else
            {
                Console.WriteLine("No load errors were reported.");
            }
        }
    }
}