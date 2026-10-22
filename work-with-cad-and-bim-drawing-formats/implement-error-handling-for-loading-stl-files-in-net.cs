using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder input STL file path
            string inputPath = "input.stl";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Configure load options to ignore errors and collect them
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the STL file with error handling
            Image cadImage = Image.Load(inputPath, loadOptions);

            // Check for any errors captured during loading
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.Error.WriteLine("Errors encountered while loading the STL file:");
                foreach (var error in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"- {error.Message}");
                }
            }
            else
            {
                Console.WriteLine("STL file loaded successfully without errors.");
            }

            // Example: export the loaded CAD image to PNG
            string outputPath = Path.Combine(outputDir, "exported.png");

            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 800,
                PageHeight = 600
                // No Layout property; Layouts can be left null or set to specific layout names if needed
            };

            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Exported PNG saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An exception occurred: {ex.Message}");
        }
    }
}