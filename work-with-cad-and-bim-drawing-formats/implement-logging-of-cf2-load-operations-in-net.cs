using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Path to the CF2 file to be loaded
        string inputFile = "sample.cf2";

        // Verify that the input file exists
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Configure load options to ignore errors and collect diagnostics
        var loadOptions = new LoadOptions();
        loadOptions.IgnoreErrors = true;

        try
        {
            using (Image cadImage = Image.Load(inputFile, loadOptions))
            {
                Console.WriteLine($"Successfully loaded CF2 file: {inputFile}");

                // Log any errors or warnings captured during the load operation
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.WriteLine("Load diagnostics:");
                    foreach (RenderResult result in loadOptions.Errors)
                    {
                        Console.WriteLine($"- {result.Message} (Code: {result.RenderCode})");
                    }
                }
                else
                {
                    Console.WriteLine("No load errors reported.");
                }

                // Additional processing of cadImage can be performed here if needed
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while loading CF2 file: {ex.Message}");
        }
    }
}