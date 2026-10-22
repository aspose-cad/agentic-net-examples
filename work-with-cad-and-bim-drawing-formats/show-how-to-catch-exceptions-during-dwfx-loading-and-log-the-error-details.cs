using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the DWFx file to be loaded
            string inputFilePath = "sample.dwf";

            // Verify that the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Configure load options to ignore errors during loading
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the DWFx file using Aspose.CAD.Image.Load with the specified options
                using (Image dwfImage = Image.Load(inputFilePath, loadOptions))
                {
                    // At this point the file is loaded successfully.
                    // Additional processing can be performed here if needed.
                }

                // After loading, inspect any non-fatal errors collected in LoadOptions.Errors
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Non-fatal errors encountered during loading:");
                    foreach (RenderResult error in loadOptions.Errors)
                    {
                        Console.WriteLine($"- {error.Message} (Code: {error.RenderCode})");
                    }
                }
                else
                {
                    Console.WriteLine("File loaded without non-fatal errors.");
                }
            }
            catch (Exception ex)
            {
                // Log exception details if loading fails
                Console.WriteLine("An exception occurred while loading the DWFx file:");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}