using System;
using System.IO;
using Aspose.CAD;

namespace AsposeCadObjLoadLogging
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder path to the OBJ file to be loaded
            string inputPath = "sample.obj";

            // Verify that the input file exists before attempting to load
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Configure load options to ignore errors and collect them for logging
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the OBJ file using Aspose.CAD.Image.Load with the specified options
                using (Image cadImage = Image.Load(inputPath, loadOptions))
                {
                    Console.WriteLine($"Successfully loaded OBJ file: {inputPath}");
                    Console.WriteLine($"Image dimensions: {cadImage.Width}x{cadImage.Height}");

                    // Log any warnings or errors captured during loading
                    if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                    {
                        Console.WriteLine("Load warnings/errors:");
                        foreach (var errorInfo in loadOptions.Errors)
                        {
                            Console.WriteLine($"- {errorInfo.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No load errors reported.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during the load operation
                Console.Error.WriteLine($"Failed to load OBJ file: {ex.Message}");
            }
        }
    }
}