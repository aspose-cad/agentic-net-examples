using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadLoadErrorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to a non‑existent or invalid DXB file
            string filePath = "invalid.dxb";

            // Guard against missing file
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                // Continue to demonstrate load error handling with a non‑existent path
            }

            // Configure load options to ignore errors during loading
            var loadOptions = new LoadOptions();
            loadOptions.IgnoreErrors = true;

            try
            {
                // Attempt to load the DXB file with the specified options
                using (CadImage cadImage = (CadImage)Image.Load(filePath, loadOptions))
                {
                    // If loading succeeds, you can work with the CadImage here
                    Console.WriteLine("CAD file loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                // Catch any exception thrown during loading
                Console.WriteLine($"Exception caught while loading CAD file: {ex.Message}");
            }

            // After the load attempt, inspect the collected load errors
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.WriteLine("Load diagnostics:");
                foreach (RenderResult error in loadOptions.Errors)
                {
                    Console.WriteLine($"- Message: {error.Message}");
                    Console.WriteLine($"  Code   : {error.RenderCode}");
                }
            }
            else
            {
                Console.WriteLine("No load errors were reported.");
            }
        }
    }
}