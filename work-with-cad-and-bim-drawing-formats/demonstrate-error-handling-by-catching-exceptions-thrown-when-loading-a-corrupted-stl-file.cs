using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the corrupted STL file
        string inputPath = "corrupted_model.stl";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Configure load options to not ignore errors
        var loadOptions = new LoadOptions();
        loadOptions.IgnoreErrors = false;

        try
        {
            // Attempt to load the STL file with the specified options
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                // If loading succeeds, the using block will dispose the image
                Console.WriteLine("STL file loaded successfully.");
            }
        }
        catch (Exception ex)
        {
            // Handle loading exceptions
            Console.WriteLine("Exception caught while loading STL file: " + ex.Message);

            // Inspect detailed errors collected in LoadOptions
            foreach (RenderResult error in loadOptions.Errors)
            {
                Console.WriteLine($"Error Message: {error.Message}");
                Console.WriteLine($"Error Code: {error.RenderCode}");
            }
        }
    }
}