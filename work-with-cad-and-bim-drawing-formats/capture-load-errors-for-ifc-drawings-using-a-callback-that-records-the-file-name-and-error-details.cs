using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to the IFC file
        string inputPath = "sample.ifc";

        // Guard against missing input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Configure load options to ignore errors and continue loading
        var loadOptions = new LoadOptions();
        loadOptions.IgnoreErrors = true;

        // Note: Aspose.CAD does not provide a load‑error callback mechanism.
        // Errors can be inspected after the load via loadOptions.Errors.

        try
        {
            // Load the CAD image with the specified options
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                Console.WriteLine("CAD file loaded successfully.");
                // Additional processing can be done here if needed
            }
        }
        catch (Exception ex)
        {
            // Handle unexpected exceptions during loading
            Console.WriteLine($"Exception while loading CAD file: {ex.Message}");
        }

        // After the load attempt, inspect any recorded load errors
        if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
        {
            Console.WriteLine("Load errors:");
            foreach (RenderResult error in loadOptions.Errors)
            {
                Console.WriteLine($"File: {inputPath}");
                Console.WriteLine($"Message: {error.Message}");
                Console.WriteLine($"Code: {error.RenderCode}");
            }
        }
        else
        {
            Console.WriteLine("No load errors reported.");
        }
    }
}