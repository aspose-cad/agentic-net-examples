using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the source DWT file and the destination PNG file
            string inputFile = "input.dwt";
            string outputFile = "output.png";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create LoadOptions without specifying an encoding.
            // Leaving SpecifiedEncoding unset enables automatic code‑page detection.
            var loadOptions = new LoadOptions
            {
                // Optional: continue loading even if non‑critical errors occur.
                IgnoreErrors = true
            };

            // Load the CAD drawing using the options.
            // The returned Image is a CadImage.
            var cadImage = (CadImage)Image.Load(inputFile, loadOptions);

            // Report any errors that were captured during loading.
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                foreach (var err in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {err.Message}");
                }
            }

            // Prepare PNG export options.
            var pngOptions = new PngOptions();

            // Save the drawing as PNG.
            cadImage.Save(outputFile, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}