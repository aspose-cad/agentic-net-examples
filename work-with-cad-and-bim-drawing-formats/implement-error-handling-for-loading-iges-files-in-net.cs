using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the IGES file
        string inputPath = "sample.iges";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Configure load options to ignore errors and collect them
        var loadOptions = new LoadOptions();
        loadOptions.IgnoreErrors = true;

        try
        {
            // Load the IGES file with the specified options
            var cadImage = Image.Load(inputPath, loadOptions);

            // Check if any errors were reported during loading
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.WriteLine("The file was loaded with the following warnings:");
                foreach (var error in loadOptions.Errors)
                {
                    Console.WriteLine("- " + error.Message);
                }
            }
            else
            {
                Console.WriteLine("File loaded successfully without warnings.");
            }

            // Optional: Save the loaded image to PNG format
            string outputPath = "output.png";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var pngOptions = new PngOptions();
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine("Image saved to " + outputPath);
        }
        catch (Exception ex)
        {
            // Handle loading exceptions
            Console.WriteLine("Failed to load IGES file:");
            Console.WriteLine(ex.Message);
        }
    }
}