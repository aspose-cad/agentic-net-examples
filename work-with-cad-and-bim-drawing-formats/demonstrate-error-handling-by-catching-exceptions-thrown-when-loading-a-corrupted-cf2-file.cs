using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadErrorHandlingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the potentially corrupted CF2 file
            string inputFilePath = "corrupted_file.cf2";

            // Guard against missing input file
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Initialize load options
            var loadOptions = new LoadOptions
            {
                // Set to false to let the loader throw an exception on errors
                IgnoreErrors = false
            };

            try
            {
                // Attempt to load the CAD file with the specified options
                using (Image cadImage = Image.Load(inputFilePath, loadOptions))
                {
                    // If loading succeeds, you can work with the image here
                    Console.WriteLine("CAD file loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                // An exception was thrown during loading
                Console.WriteLine($"Exception while loading CAD file: {ex.Message}");

                // Inspect detailed load errors collected in LoadOptions.Errors
                IList<RenderResult> loadErrors = loadOptions.Errors;
                if (loadErrors != null && loadErrors.Count > 0)
                {
                    Console.WriteLine("Load errors details:");
                    foreach (RenderResult error in loadErrors)
                    {
                        Console.WriteLine($"- Code: {error.RenderCode}, Message: {error.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("No detailed load errors were captured.");
                }
            }
        }
    }
}